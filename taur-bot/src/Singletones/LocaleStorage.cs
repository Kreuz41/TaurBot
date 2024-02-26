using System.Text.Json;

namespace taur_bot.Singletones;

public class LocaleStorage
{
    private Dictionary<int, Dictionary<string, string>> Locale = new();
    public LocaleStorage()
    {
        FillLocale();
    }

    private void FillLocale()
    {
        var locale = File.ReadAllText("src/Prefabs/langs/en.json");
        Locale.Add(0, JsonSerializer.Deserialize<Dictionary<string, string>>(locale)!);

        locale = File.ReadAllText("src/Prefabs/langs/ru.json");
        Locale.Add(1, JsonSerializer.Deserialize<Dictionary<string, string>>(locale)!);
    }

    public string? GetLocaleText(int locale, string key)
    {
        Locale.TryGetValue(locale, out var lang);
        if (lang == null) return null;

        lang.TryGetValue(key, out var value);
        return value;
    }
}