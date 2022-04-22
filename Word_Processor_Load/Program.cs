using Word_Processor_Load.Model;

namespace Word_Processor_Load
{
    class Program
    {
        static void Main()
        {
            var provider = new Provider();
            var words = new List<Word>();

            //собираем и считаем слова длины >=3 и <=20 из текстового файла
            using (var sr = new StreamReader("Text.txt"))
            {
                while (true)
                {
                    var line = sr.ReadLine();

                    if (line == null)
                    {
                        break;
                    }

                    var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    
                    foreach (var x in split)
                    {
                        if (x.Length >= 3 && x.Length <= 20)
                        {
                            var word = words.FirstOrDefault(word => word.Name == x);

                            if (word != null)
                            {
                                word.Mentions++;
                            }
                            else words.Add(new Word(x, 1));
                        }
                    }
                }
            }

            //импортируем все слова с количеством упоминаний >= 4
            provider.Import(words.Where(x => x.Mentions >= 4));
        }
    }
}