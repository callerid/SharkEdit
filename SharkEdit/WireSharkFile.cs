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
        public void Process(bool filter_out_registers = false, 
                            bool filter_out_200_oks = false,
                            bool filter_out_options = false,
                            bool filter_out_notifys = false)
        {
            int bytes_left = FileContents.Length;

            SetProgressBarMax(bytes_left);

            int offset = 0;
            int progress = 0;
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

                bool filter_this_packet = false;
                string sip_type = "";
                string seq_method = "";

                // Packet type
                int type_index = 0x2A + offset;

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
                    seq_method = packet_ascii.Substring(sequence_pos, 20);
                }

                // If register packet then filter
                if (filter_out_registers && GetSIPFromString(sip_type) == "Register") filter_this_packet = true;
                if (filter_out_notifys && GetSIPFromString(sip_type) == "Notify") filter_this_packet = true;

                // If 200 OK's then filter if not seq to an invite
                if (filter_out_200_oks && GetSIPFromString(sip_type) == "200 OK" && (!seq_method.Contains("INVITE") && !seq_method.Contains("BYE") && !seq_method.Contains("CANCEL"))) filter_this_packet = true;
                if (filter_out_options && GetSIPFromString(sip_type) == "Options" && (!seq_method.Contains("INVITE") && !seq_method.Contains("BYE") && !seq_method.Contains("CANCEL"))) filter_this_packet = true;

                // RTP checking
                bool rtp = packet_length == 214 || packet_length == 218;
                
                if (!filter_this_packet)
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
                    SIPTypes.Add(GetSIPFromString(sip_type));
                }
                
                
                // Update counter to watch of end of file
                bytes_left -= (packet_length + HEADER_SIZE);

                // Update offset
                offset += (packet_length + HEADER_SIZE);

                // Update progress bar
                progress += packet_length;
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
        public int[] GetPacketTimestamp(int packet_index)
        {

            // Create temp packet to read timestamp from
            byte[] file_contents = GetPacket(packet_index);

            // Get timestamp from header
            byte[] timestamp_bytes = new byte[4] { file_contents[0], file_contents[1], file_contents[2], file_contents[3] };
            byte[] u_timestamp_bytes = new byte[4] { file_contents[4], file_contents[5], file_contents[6], file_contents[7] };
            int timestamp_seconds = BitConverter.ToInt32(timestamp_bytes, 0);
            int u_timestamp_seconds = BitConverter.ToInt32(u_timestamp_bytes, 0);

            return new int[] { timestamp_seconds, u_timestamp_seconds };

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
            if (piece.Contains("INVITE")) return "Invite";
            if (piece.Contains("100 T")) return "100 Trying";
            if (piece.Contains("180 R")) return "180 Ringing";
            if (piece.Contains("ACK")) return "Acknowledgment";
            if (piece.Contains("BYE")) return "Bye";
            if (piece.Contains("200 OK") || piece.Contains("200 Ok")) return "200 OK";
            if (piece.Contains("OPT")) return "Options";
            if (piece.Contains("CANCEL")) return "Cancel";
            if (piece.Contains("NOTIFY")) return "Notify";
            return piece;
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
