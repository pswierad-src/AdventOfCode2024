using System.Text;

namespace Day9;

public class Processor
{
    public async Task Run(string path)
    {
        var input = (await File.ReadAllLinesAsync(path))[0].ToCharArray();

        List<long> disk = [];

        long id = 0;

        for (long i = 0; i < input.Length; i++)
        {
            if (i % 2 == 0)
            {
                disk.AddRange(Enumerable.Repeat(id, (int)char.GetNumericValue(input[i])));
                id++;
            }
            else
            {
                disk.AddRange(Enumerable.Repeat((long)-1, (int)char.GetNumericValue(input[i])));
            }
        }

        Task1(disk);
        Task2(disk);

    }

    private void Task1(List<long> input)
    {
        var disk = new List<long>(input);

        for (var i = 0; i < disk.Count; i++)
        {
            if(disk[i] != -1)
                continue;

            var lastSector = GetLastSectorWithIndex(disk);

            if(lastSector.index == -int.MaxValue)
                continue;

            if (lastSector.index <= i)
                break;

            (disk[i], disk[lastSector.index]) = (disk[lastSector.index], disk[i]);
        }

        Console.WriteLine($"New checksum after segmenting: {CalculateChecksum(disk)}");
    }

    private void Task2(List<long> input)
    {
        var disk = new List<long>(input);

        var lastFile = input.Max();

        Console.WriteLine("This will take a while :)");
        for (var i = lastFile; i >= 0; i--)
        {
            var file = GetFileRange(disk, i);

            var freeIndex = GetFirstAvailableSpace(disk, file.length);

            if(freeIndex is null || freeIndex > file.indexFrom)
                continue;

            for (var j = 0; j < file.length; j++)
            {
                disk[freeIndex.Value + j] = i;
                disk[file.indexFrom + j] = -1;
            }

            //Console.WriteLine(string.Join(string.Empty, disk).Replace("-1","."));
        }

        Console.WriteLine($"New checksum after moving files: {CalculateChecksum(disk)}");
    }

    private long CalculateChecksum(List<long> input)
    {
        long checksum = 0;

        for (int i = 0; i < input.Count; i++)
        {
            if(input[i] == -1)
                continue;

            checksum += input[i] * i;
        }

        return checksum;
    }

    private (long sector, int index) GetLastSectorWithIndex(List<long> input)
    {
        for (int i = input.Count-1; i >= 0; i--)
        {
            if (input[i] != -1)
            {
                return (input[i], i);
            }
        }

        return (-int.MaxValue, -int.MaxValue);
    }

    private (int indexFrom, int indexTo, int length) GetFileRange(List<long> input, long fileName)
    {
        var file = input.Select(
            (s, i) =>
            {
                if (s == fileName)
                    return i;

                return 0;
            }).Where(s => s != 0).ToList();

        return (file.Min(), file.Max(), file.Count);
    }

    private int? GetFirstAvailableSpace(List<long> input, int length)
    {
        int? indexFrom = null;
        int currentLength = 0;

        for (var i = 0; i < input.Count; i++)
        {
            if (input[i] == -1)
            {
                indexFrom ??= i;

                currentLength++;

                if (currentLength == length)
                    return indexFrom.Value;

                continue;
            }

            indexFrom = null;
            currentLength = 0;
        }

        return null;
    }
}
