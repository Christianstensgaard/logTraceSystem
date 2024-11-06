//- Christian Leo Stensgaard Jørgensen
namespace BitToolbox;
public static class BitHandler{
  public static void SetBit(byte[] byteArray, int bitIndex, bool value)
  {
    int bytePos = bitIndex / 8;
    int bitPos = bitIndex % 8;

    if (bytePos < 0 || bytePos >= byteArray.Length)
        throw new ArgumentOutOfRangeException(nameof(bitIndex), "Bit index is out of range.");

    byteArray[bytePos] = value ?
        (byte)(byteArray[bytePos] | (byte)(1 << bitPos)) :
        (byte)(byteArray[bytePos] & ~(byte)(1 << bitPos));
  }
}