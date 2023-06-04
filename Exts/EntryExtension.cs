
public class EntryExtension
{
	public static async Task<string> GetPublicIpAddress()
		=> await new HttpClient().GetStringAsync("https://api.ipify.org");

}
