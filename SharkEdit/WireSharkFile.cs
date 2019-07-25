using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkEdit
{
    public class WireSharkFile
    {

        // Contants
        private const int HEADER_SIZE = 16;

        // All packets keeper
        public List<byte[]> Packets = new List<byte[]>();
        private byte[] GlobalHeader;
        
        // Constuctor
        public WireSharkFile(byte[] file_contents, byte[] global_header)
        {

            GlobalHeader = global_header;

            // Loop through all packets and pull them out
            // and put into packets
            int bytes_left = file_contents.Length;
            while (bytes_left > 1)
            {
                // Header Total Length = 16 bytes
                // Header[0]-[3] => Time Stamps
                // Header[4]-[7] => Time Stamp milli seconds
                // Header[8]-[11] => Length of Packet (octects)
                // Header[12]-[15] => Length of Packet (actual)                

                // Get length of the packet
                byte[] length_bytes = new byte[4] { file_contents[12], file_contents[13], file_contents[14], file_contents[15] };
                int packet_length = BitConverter.ToInt32(length_bytes, 0);

                // Capture packet for altering later
                byte[] full_packet = new byte[packet_length + 16];
                full_packet[0] = file_contents[0];
                full_packet[1] = file_contents[1];
                full_packet[2] = file_contents[2];
                full_packet[3] = file_contents[3];
                full_packet[4] = file_contents[4];
                full_packet[5] = file_contents[5];
                full_packet[6] = file_contents[6];
                full_packet[7] = file_contents[7];
                full_packet[8] = file_contents[8];
                full_packet[9] = file_contents[9];
                full_packet[10] = file_contents[10];
                full_packet[11] = file_contents[11];
                full_packet[12] = file_contents[12];
                full_packet[13] = file_contents[13];
                full_packet[14] = file_contents[14];
                full_packet[15] = file_contents[15];

                for (int i = 16; i < packet_length + 16; i++)
                {
                    full_packet[i] = file_contents[i];
                }
                Packets.Add(full_packet);

                // Update counter to watch of end of file
                bytes_left = file_contents.Length - (packet_length + HEADER_SIZE);

                // Refresh contents to stay up-to-date with loop
                byte[] buffer = new byte[bytes_left];
                for (int i = 0; i < bytes_left; i++)
                {
                    buffer[i] = file_contents[i + packet_length + HEADER_SIZE];
                }
                file_contents = new byte[bytes_left];
                buffer.CopyTo(file_contents, 0);
                
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
            }

            foreach(byte[] packet in Packets)
            {
                foreach (byte b in packet)
                {
                    output[output_index] = b;
                    output_index++;
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

    }
}
