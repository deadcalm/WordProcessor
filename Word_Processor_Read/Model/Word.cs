namespace Word_Processor_Read.Model
{
    internal class Word
    {
        public Word(string name, int numberOfMention)
        {
            Name = name;
            NumberOfMention = numberOfMention;
        }

        public string Name { get; set; }
        public int NumberOfMention { get; set; }
    }

}

