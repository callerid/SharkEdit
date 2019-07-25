using System;
using System.Collections.Generic;
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
        
        // Constuctor
        public WireSharkFile(byte[] file_contents)
        {
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
                byte[] full_packet = new byte[packet_length];
                for (int i = 0; i < packet_length; i++)
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

    }
}
