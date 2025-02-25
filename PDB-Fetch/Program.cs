
class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        string filePath = args[0];
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"{filePath} does not exist");
            return;
        }

        string[] ProteinList = File.ReadAllLines(filePath);

        foreach (string ProteinID in ProteinList)
        {
            string url = $"https://files.rcsb.org/download/{ProteinID}.pdb";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    string outputFilePath = $"{ProteinID}.pdb";
                    File.WriteAllText(outputFilePath, content);
                    Console.WriteLine($"{ProteinID}.pdb retrieved");
                }
                else
                {
                    Console.WriteLine($"PDB {ProteinID} not found");
                    Console.WriteLine($"Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error for {ProteinID}: {e.Message}");
            }
        }
    }
}

