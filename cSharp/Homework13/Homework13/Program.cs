using System;
using System.Collections.Generic;

namespace Homework13
{
    class Program
    {
        static void Main(string[] args)
        {
            string originalText = "Вот дом, Который построил Джек. А это пшеница, " +
                                  "Которая в темном чулане хранится В доме, Который построил Джек. " +
                                  "А это веселая птица-синица, Которая часто ворует пшеницу, " +
                                  "которая в темном чулане хранится в доме Который построил Джек.";

            string lowerCaseText = originalText.ToLower();

            //разделители: пробел, знаки препинания
            char[] delimiters = { ' ', ',', '.', '!', '?', ';', ':', '\n', '\r' };
            string[] wordsArray = lowerCaseText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            // подсчёт частоты слов с помощью Dictionary
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();

            foreach (string currentWord in wordsArray)
            {
                if (wordFrequency.ContainsKey(currentWord))
                    wordFrequency[currentWord]++;
                else
                    wordFrequency[currentWord] = 1;
            }

            Console.WriteLine("слов в тексте:\n");
            Console.WriteLine("Слово\t\t\tКоличество");

            foreach (KeyValuePair<string, int> entry in wordFrequency)
            {
                Console.WriteLine($"{entry.Key,-20}\t{entry.Value}");
            }

            Console.WriteLine($"\nВсего слов: {wordsArray.Length}, из них уникальных: {wordFrequency.Count}");
        }
    }
}