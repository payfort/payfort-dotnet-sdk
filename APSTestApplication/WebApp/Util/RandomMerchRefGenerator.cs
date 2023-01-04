namespace WebApp.Util;

public class RandomMerchRefGenerator
{
    private static readonly Random getrandom = new Random();

    private static int GetRandomNumber(int min, int max)
    {
        lock(getrandom) // synchronize
        {
            return getrandom.Next(min, max);
        }
    }

    public static string GetRandomMerchRef()
    {
        return "merchanttest-1000" + GetRandomNumber(100000, 9000000);
    }
}