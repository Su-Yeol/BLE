namespace Yeol
{
    public class Simulation : PayloadBase
    {
        public Simulation(byte[] stream) : base(stream)
        {
        }

        public Simulation(int payloadLength) : base(payloadLength)
        {
        }
    }
}