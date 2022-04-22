namespace Word_Processor_Load.Model
{
    internal class Word
    {
        public Word(string name, int mentions)
        {
            Name = name;
            Mentions = mentions;
        }
        public string Name { get; set; }
        public int Mentions { get; set; }
    }
}
