﻿public class DayOne
{
    private readonly DataFetcher _dataFetcher;
    public DayOne(DataFetcher dataFetcher)
    {
        _dataFetcher = dataFetcher ?? throw new ArgumentNullException(nameof(dataFetcher));
    }
    
    public async Task Run()
    {
        await _dataFetcher.GetAndStoreData(2022, 1);
        Part1();
        Part2();
    }

    private void Part1()
    {

    }

    private void Part2()
    {

    }
}