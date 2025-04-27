using System;

namespace Core.Specifications;

public class ProductSpecParams
{
    public const int MaxPageSize = 50;
    public int PageIndex = 1;
    private int _pageSize = 6;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    private List<string> _brands = [];

    public List<string> Brands
    {
        get => _brands;
        set => _brands = value.SelectMany(b => b.Split(',',
        StringSplitOptions.RemoveEmptyEntries)).ToList();
    }

    private List<string> _types = [];
    public List<string> Types
    {
        get => _types;
        set => _types = value.SelectMany(t => t.Split(',',
        StringSplitOptions.RemoveEmptyEntries)).ToList();
    }

    public string? Sort { get; set; }
    private string? _search;
    public string Search
    {
        get => _search ?? string.Empty;
        set => _search = value.ToLower();
    }
}
