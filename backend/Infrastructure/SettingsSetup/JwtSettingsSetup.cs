using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.SettingsSetup;

public class JwtSettingsSetup(IConfiguration configuration) : IConfigureOptions<JwtSettings>
{
    private const string SectionName = "JwtSettings";

    public void Configure(JwtSettings options)
    {
        configuration.GetSection(SectionName).Bind(options);

    }
}
