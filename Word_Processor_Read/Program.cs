namespace Word_Processor_Read
{
    class Program
    {
        private static void Main(string[] args)
        {
            var provider = new Provider();

            var prefix = Console.ReadLine();
            
            if (prefix is null)
            {
                throw new Exception("prefix is null");
            }

            var words = provider.Read(prefix);

            foreach (var word in words)
            {
                Console.WriteLine(word);
            }
        }
    }
}
