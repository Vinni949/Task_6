using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Collections;

namespace Task_6
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = @"input.txt";
            IsInputFile(input);
            Console.WriteLine("Загружаю число из файла...");
            int number = ReadFile(input);                           
            int[][] result = new int[ReturnGroupCount(number)][];      
            result[0] = new int[] { 1 };                               

            Console.WriteLine("Внимание! Число в файле = " + number);
            Console.WriteLine();
            Console.WriteLine("Выберите режим работы: \n" +
                "1 - Показать только кол-во групп\n" +
                "2 - Выполнить алгоритм подсчета групп чисел методом перебора значений\n" +
                "с последующей архивацией или сохранением в файл\n");
            switch (Console.ReadLine())
            {
             
                case "1":
                    int[] range = new int[number - 2];
                    Console.WriteLine("Групп чисел: " + ReturnGroupCount(number));
                    break;

                case "2":
                    Console.WriteLine("Формирую массив чисел от 1 до вашего числа");
                    range = new int[number - 2];
                    range = Enumerable.Range(2, number).ToArray();

                    for (int i = 1; i < result.Length; i++)
                    {
                        result[i] = ReturnList(range);             
                        range = range.Except<int>(result[i]).ToArray();    
                    }

                    Console.WriteLine("Производим запись результатов в файл output2.txt");
                    string file = "output2.txt";
                    SaveToFile(file, result);

                    Console.WriteLine("Сохранение в файл завершено!\n");
                    Console.WriteLine("Заархивировать полученный файл? \n" +
                        "1 - Да\n" +
                        "0 - Нет, просто выйти из программы");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            Compressig(file, "archivedOutput2.zip");
                            Console.WriteLine("Файл заархивирован. Ищите его в папке с exe файлом");
                            break;
                        case "0":
                            break;
                    }
                    break;

          }
            Console.ReadLine();
        }

        static BitArray ErastofenBool(BitArray list)
        {
            BitArray arr = new BitArray(list);
            for (int i = 2; i * 2 < arr.Length; i++)
            {
                if (!list[i])
                {
                    for (int j = i * 2; j < arr.Length; j += i)
                    {
                        arr[j] = true;
                    }
                }
            }
            return arr;
        }

        static int[] ReturnList(int[] array)
        {
            int[] buffArray = new int[array.Length];
            array.CopyTo(buffArray, 0);
            bool flag = true;
            for (int i = 0; i * 2 < buffArray.Length; i++)
            {
                if (buffArray[i] != 0)
                {
                    for (int j = i; j < buffArray.Length; j++)
                    {
                        if (buffArray[j] % buffArray[i] == 0 && buffArray[j] / buffArray[i] > 1)
                        {
                            buffArray[j] = 0;
                            flag = false;
                        }
                    }
                }
            }
            if (!flag) return ReturnList(buffArray);
            else return buffArray.Where(x => x != 0).ToArray();        
        }

        static int ReadFile(string path)
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                return int.Parse(sr.ReadLine());
            }
        }

        static void IsInputFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл данных input.txt не найден. Создаю файл по умолчанию");
                using (StreamWriter sw = new StreamWriter(new FileStream("input.txt", FileMode.Create, FileAccess.Write)))
                {
                    sw.WriteLine("1000");
                }
            }
        }

        static void Compressig(string source, string output)
        {
            if (!File.Exists(source)) Console.WriteLine("Файл не существует! Создайте файл для архивации или проверьте правильность пути");
            else
            {
                using (FileStream fs = new FileStream(source, FileMode.Open))
                {
                    using (FileStream nf = File.Create(output))
                    {
                        using (GZipStream gs = new GZipStream(nf, CompressionMode.Compress))
                        {
                            fs.CopyTo(nf);
                        }
                    }
                }
            }
        }

        static void SaveToFile(string name, int[][] array)
        {
            string fileName = name;
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            using (StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                for (int i = 0; i < array.Length; i++)
                {
                    sw.WriteLine(PrintArray(array[i]));
                    sw.Flush();
                }
            }
        }
   
        static BitArray ReversBool(BitArray arr1, BitArray arr2)
        {
            BitArray result = new BitArray(arr1);
            for (int i = 0; i < result.Length; i++)
            {
                if (arr1[i] == arr2[i] && !arr1[i]) result[i] = !arr1[i];
            }
            return result;
        }

        
        static int ReturnGroupCount(int number)
        {
            int count = 1;
            while (number > 1)
            {
                number /= 2;
                count++;
            }
            return count;
        }

        
        static string PrintArray(int[] array)
        {
            string result = null;
            foreach (int item in array)
            {
                result += item + " ";
            }
            return result;
        }

       
        static int[] FromBitToInt(BitArray array)
        {
            int[] result = new int[array.Length];
            for (int j = 1; j < array.Length; j++)
            {
                if (!array[j])
                    result[j] = j;
                else
                    result[j] = 0;
            }
            return result;
        }


    }
}
