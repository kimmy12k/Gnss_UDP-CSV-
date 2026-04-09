using System;

namespace UDPMode
{
    internal static class HilPacket
    {
        public const int PACKET_SIZE = 216;
        private const int RESERVED_SIZE = 16;

        // 전체 25개 double 파라미터 (매뉴얼 p.255 Table 16-2)
        //
        // 16B 예약 + 25 × 8B = 216B

        //
        public static byte[] Build(
            double elapsedTime,
            double posX, double posY, double posZ,
            double velX, double velY, double velZ,
            double accX, double accY, double accZ,
            double jerkX, double jerkY, double jerkZ,
            double yaw = 0, double pitch = 0, double roll = 0)
        {
            var buf = new byte[PACKET_SIZE];
            int offset = RESERVED_SIZE;

            // 1: ElapsedTime
            WriteDoubleBE(buf, offset, elapsedTime); offset += 8;

            // 2~4: Position (ECEF)
            WriteDoubleBE(buf, offset, posX); offset += 8;
            WriteDoubleBE(buf, offset, posY); offset += 8;
            WriteDoubleBE(buf, offset, posZ); offset += 8;

            // 5~7: Velocity
            WriteDoubleBE(buf, offset, velX); offset += 8;
            WriteDoubleBE(buf, offset, velY); offset += 8;
            WriteDoubleBE(buf, offset, velZ); offset += 8;

            // 8~10: Acceleration
            WriteDoubleBE(buf, offset, accX); offset += 8;
            WriteDoubleBE(buf, offset, accY); offset += 8;
            WriteDoubleBE(buf, offset, accZ); offset += 8;

            // 11~13: Jerk
            WriteDoubleBE(buf, offset, jerkX); offset += 8;
            WriteDoubleBE(buf, offset, jerkY); offset += 8;
            WriteDoubleBE(buf, offset, jerkZ); offset += 8;

            // 14~16: Attitude (Yaw, Pitch, Roll)
            WriteDoubleBE(buf, offset, yaw); offset += 8;
            WriteDoubleBE(buf, offset, pitch); offset += 8;
            WriteDoubleBE(buf, offset, roll); offset += 8;


            return buf;
        }

        private static void WriteDoubleBE(byte[] buf, int offset, double value) // 255 page Bigendian으로
        {
            byte[] bytes = BitConverter.GetBytes(value);
            //if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            Buffer.BlockCopy(bytes, 0, buf, offset, 8);
        }
    }
}
