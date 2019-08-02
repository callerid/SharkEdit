using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkEdit
{
    public class WireSharkFile
    {

        // Contants
        private const int HEADER_SIZE = 16;

        // All packets keeper
        public List<byte[]> Packets = new List<byte[]>();
        public List<string> SIPTypes = new List<string>();
        public byte[] FileContents;
        private byte[] GlobalHeader;
        private decimal StartTimeStamp = -1;

        // Events
        // -- Set progressbar max
        public delegate void SetProgressBarMaxDelegate(int value);
        public SetProgressBarMaxDelegate SetProgressBarMax;

        // -- Update progressbar value
        public delegate void UpdateProgressBarDelegate(int value);
        public UpdateProgressBarDelegate UpdateProgressBar;
        
        // Constuctor
        public WireSharkFile(byte[] file_contents, byte[] global_header)
        {
            FileContents = file_contents;
            GlobalHeader = global_header;

            Packets = new List<byte[]>();
        }

        // Pull out packets and place into packets list
        public void Process(bool filter_out_non_essentials = false,
                            bool filter_out_options = false,
                            bool filter_out_all_RTP = false)
        {
            int bytes_left = FileContents.Length;

            SetProgressBarMax(bytes_left);

            int offset = 0;
            int progress = 0;
            int rtp_count = 0;
            bool first_packet = true;
            int packet = 1;
            while (bytes_left > 1)
            {
                // Header Total Length = 16 bytes
                // Header[0]-[3] => Time Stamps
                // Header[4]-[7] => Time Stamp milli seconds
                // Header[8]-[11] => Length of Packet (octects)
                // Header[12]-[15] => Length of Packet (actual) 

                // Get length of the packet
                byte[] length_bytes = new byte[4] { FileContents[12 + offset], FileContents[13 + offset],
                                                FileContents[14 + offset], FileContents[15 + offset] };

                int packet_length = BitConverter.ToInt32(length_bytes, 0);

                if (StartTimeStamp == -1)
                {
                    byte[] timestamp_bytes = new byte[4] { FileContents[0], FileContents[1], FileContents[2], FileContents[3] };
                    byte[] u_timestamp_bytes = new byte[4] { FileContents[4], FileContents[5], FileContents[6], FileContents[7] };
                    int timestamp_seconds = BitConverter.ToInt32(timestamp_bytes, 0);
                    int u_timestamp_seconds = BitConverter.ToInt32(u_timestamp_bytes, 0);

                    string timestamp = timestamp_seconds + "." + u_timestamp_seconds;
                    StartTimeStamp = decimal.Parse(timestamp);

                }

                bool filter_this_packet = false;
                string sip_type = "";
                string seq_method = "";

                // Packet type
                int type_index = 0x2A + offset;

                if (type_index > FileContents.Length) break;

                byte[] type_bytes = new byte[20];
                for (int i = 0; i < type_bytes.Length; i++)
                {
                    type_bytes[i] = FileContents[i + HEADER_SIZE + type_index];
                }
                sip_type = Encoding.ASCII.GetString(type_bytes);

                int sequence_pos = 0;
                string packet_ascii = Encoding.ASCII.GetString(FileContents, offset, packet_length);

                if (packet_ascii.IndexOf("CSeq:") > -1)
                {
                    sequence_pos = packet_ascii.IndexOf("CSeq");
                    seq_method = packet_ascii.Substring(sequence_pos, packet_length > sequence_pos + 20 ? 20 : packet_length - sequence_pos);
                }

                string type_piece = packet_ascii.Substring(packet_length >= 66 ? 66 - (first_packet ? 8 : 0) : 0, packet_length > 66 + 40 ? 40 : packet_length - 66 > 0 ? packet_length - 66 : packet_length);

                string full_sip_method = GetSIPFromString(sip_type);
                if (full_sip_method.Contains("err:"))
                {
                    if (!GetSIPFromString(type_piece).Contains("err:"))
                    {
                        full_sip_method = GetSIPFromString(type_piece);
                    }
                    else
                    {
                        int attemp_index = packet_ascii.IndexOf("SIP/2.0") - 6;
                        if (attemp_index > -1)
                        {
                            type_piece = packet_ascii.Substring(attemp_index, packet_length - attemp_index > 40 ? 40 : packet_length - attemp_index);
                            full_sip_method = GetSIPFromString(type_piece);
                        }
                    }
                }

                // Type of packet
                first_packet = false;
                bool is_sip = !GetSIPFromString(type_piece).Contains("err:") || !(GetSIPFromString(sip_type).Contains("err:"));
                bool rtp = packet_length == 214 || packet_length == 218 || packet_length == 60;

                // Filter out anything that's not a SIP record or an RTP packet
                if (!is_sip && !rtp) filter_this_packet = true;

                // If register packet then filter
                if (filter_out_non_essentials && full_sip_method == "Register") filter_this_packet = true;
                if (filter_out_non_essentials && full_sip_method == "Notify") filter_this_packet = true;
                if (filter_out_non_essentials && full_sip_method == "Subscribe") filter_this_packet = true;
                if (filter_out_non_essentials && full_sip_method == "Not Acceptable") filter_this_packet = true;
                if (filter_out_non_essentials && full_sip_method == "Accepted") filter_this_packet = true;
                if (filter_out_non_essentials && full_sip_method == "Proxy Auth.") filter_this_packet = true;
                if (filter_out_non_essentials && full_sip_method == "Info") filter_this_packet = true;

                // If 200 OK's then filter if not seq to an invite
                if (filter_out_non_essentials && full_sip_method == "200 OK" && (!seq_method.Contains("INV") && !seq_method.Contains("BYE") && !seq_method.Contains("CANC"))) filter_this_packet = true;
                if (filter_out_options && full_sip_method == "Options" && (!seq_method.Contains("INV") && !seq_method.Contains("BYE") && !seq_method.Contains("CANC"))) filter_this_packet = true;
                if (filter_out_non_essentials && full_sip_method == "Acknowledgment" && (!seq_method.Contains("INV") && !seq_method.Contains("BYE") && !seq_method.Contains("CANC"))) filter_this_packet = true;

                // Check RTP filtering
                if (rtp && filter_out_all_RTP) filter_this_packet = true;

                if (seq_method.Contains("INV") || seq_method.Contains("BYE") || seq_method.Contains("CANC")) filter_this_packet = false;
                
                if (!filter_this_packet)
                {
                    // One out of every 25 rtps
                    bool skip_packet = false;
                    if (rtp)
                    {
                        if (rtp_count < 25) rtp_count++;
                        
                        if(rtp_count == 1)
                        {
                            skip_packet = false;
                        }
                        else if (rtp_count != 25)
                        {
                            skip_packet = true;
                        }
                        else
                        {
                            skip_packet = false;
                            rtp_count = 1;
                        }
                    }

                    if (!skip_packet)
                    {

                        // Fill header of this packet
                        byte[] full_packet = new byte[packet_length + 16];
                        full_packet[0] = FileContents[0 + offset];
                        full_packet[1] = FileContents[1 + offset];
                        full_packet[2] = FileContents[2 + offset];
                        full_packet[3] = FileContents[3 + offset];
                        full_packet[4] = FileContents[4 + offset];
                        full_packet[5] = FileContents[5 + offset];
                        full_packet[6] = FileContents[6 + offset];
                        full_packet[7] = FileContents[7 + offset];
                        full_packet[8] = FileContents[8 + offset];
                        full_packet[9] = FileContents[9 + offset];
                        full_packet[10] = FileContents[10 + offset];
                        full_packet[11] = FileContents[11 + offset];
                        full_packet[12] = FileContents[12 + offset];
                        full_packet[13] = FileContents[13 + offset];
                        full_packet[14] = FileContents[14 + offset];
                        full_packet[15] = FileContents[15 + offset];

                        // Fill data of this packet, start after header
                        for (int i = 16; i < packet_length + 16; i++)
                        {
                            full_packet[i] = FileContents[i + offset];
                        }

                        Packets.Add(full_packet);
                        SIPTypes.Add(rtp ? "RTP" : full_sip_method);

                    }
                }                
                
                // Update counter to watch of end of file
                bytes_left -= (packet_length + HEADER_SIZE);

                // Update offset
                offset += (packet_length + HEADER_SIZE);

                // Update progress bar
                progress += packet_length;
                packet++;
                UpdateProgressBar(progress);

                // Break out when finished
                if (bytes_left < 2) break;

            }
        }
        
        // Packet count
        public int GetPacketsCount()
        {
            return Packets.Count;
        }

        // Get a packet out of file
        public byte[] GetPacket(int packet_index)
        {
            // Create temp packet to read length from
            byte[] file_contents = new byte[Packets[packet_index].Length];
            Packets[packet_index].CopyTo(file_contents, 0);
            return file_contents;
        }

        // Get a packet length
        public int GetPacketLength(int packet_index)
        {

            // Create temp packet to read length from
            byte[] file_contents = GetPacket(packet_index);

            // Get length of the packet
            byte[] length_bytes = new byte[4] { file_contents[12], file_contents[13], file_contents[14], file_contents[15] };
            int packet_length = BitConverter.ToInt32(length_bytes, 0);

            return packet_length;
        }

        // Get both timestamps
        public string GetPacketTimestamp(int packet_index)
        {

            // Create temp packet to read timestamp from
            byte[] file_contents = GetPacket(packet_index);

            // Get timestamp from header
            byte[] timestamp_bytes = new byte[4] { file_contents[0], file_contents[1], file_contents[2], file_contents[3] };
            byte[] u_timestamp_bytes = new byte[4] { file_contents[4], file_contents[5], file_contents[6], file_contents[7] };
            int timestamp_seconds = BitConverter.ToInt32(timestamp_bytes, 0);
            int u_timestamp_seconds = BitConverter.ToInt32(u_timestamp_bytes, 0);

            string timestamp = timestamp_seconds + "." + u_timestamp_seconds;
            decimal f_timestamp = decimal.Parse(timestamp);

            return (f_timestamp - StartTimeStamp).ToString("0.000");

        }

        // Set both timestamps
        public bool SetPacketTimestamp(int packet_index, byte[] timestamp, byte[] u_timestamp)
        {
            if (timestamp.Length < 4 || u_timestamp.Length < 4) return false;

            // Create temp packet to read timestamp from
            byte[] file_contents = GetPacket(packet_index);

            file_contents[3] = timestamp[3];
            file_contents[2] = timestamp[2];
            file_contents[1] = timestamp[1];
            file_contents[0] = timestamp[0];

            file_contents[7] = u_timestamp[3];
            file_contents[6] = u_timestamp[2];
            file_contents[5] = u_timestamp[1];
            file_contents[4] = u_timestamp[0];

            Packets[packet_index] = file_contents;

            return true;

        }

        public void SetStartTime(decimal time)
        {
            StartTimeStamp = time;
        }

        // Export
        public bool WriteFile(string file_name)
        {
            int total_length = GlobalHeader.Length;
            SetProgressBarMax(total_length);

            foreach(byte[] packet in Packets)
            {
                total_length += packet.Length;
            }

            byte[] output = new byte[total_length];
            int output_index = 0;

            foreach(byte b in GlobalHeader)
            {
                output[output_index] = b;
                output_index++;
                UpdateProgressBar(output_index);
            }

            foreach(byte[] packet in Packets)
            {
                foreach (byte b in packet)
                {
                    output[output_index] = b;
                    output_index++;
                    UpdateProgressBar(output_index);
                }
            }

            try
            {
                File.WriteAllBytes(file_name, output);
            }
            catch
            {
                return false;
            }

            return true;
        }

        // Get SIP type
        private string GetSIPFromString(string piece)
        {
            if (piece.Contains("REG")) return "Register";
            if (piece.Contains("INV")) return "Invite";
            if (piece.Contains("100 T")) return "100 Trying";
            if (piece.Contains("180 R")) return "180 Ringing";
            if (piece.Contains("ACK")) return "Acknowledgment";
            if (piece.Contains("BYE")) return "Bye";
            if (piece.Contains("200 OK") || piece.Contains("200 Ok")) return "200 OK";
            if (piece.Contains("OPT")) return "Options";
            if (piece.Contains("CANCEL")) return "Cancel";
            if (piece.Contains("NOTIFY")) return "Notify";
            if (piece.Contains("Unauthor")) return "401 Unauthorized";
            if (piece.Contains("SUBS")) return "Subscribe";
            if (piece.Contains("488 N")) return "Not Acceptable";
            if (piece.Contains("202 A")) return "Accepted";
            if (piece.Contains("PRA")) return "PRACK";
            if (piece.Contains("487 R")) return "Request Cancelled";
            if (piece.Contains("407 P")) return "Proxy Auth.";
            if (piece.Contains("INFO")) return "Info";
            if (piece.Contains("SIP/2.0")) return "Unknown SIP";
            return "err:" + piece;
        }

        // Get SIP type from packets
        public string GetSIPString(int packet_index)
        {
            return SIPTypes[packet_index];
        }

        // Is RTP
        public bool IsRTP(int packet_index)
        {
            return GetPacketLength(packet_index) == 214 || GetPacketLength(packet_index)
 == 218;
        }

    }
}
