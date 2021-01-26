using Microsoft.TemplateEngine.Abstractions.TemplatesSources;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.TemplateEngine.Abstractions.Installer
{
    public interface IInstaller
    {
        /// <summary>
        /// Installer should determine if it can install specific <see cref="InstallRequest"/>.
        /// Ideally it should as far as calling backend server to determine if such identifier exists.
        /// </summary>
        Task<bool> CanInstallAsync(InstallRequest installationRequest);

        /// <summary>
        /// User can specify name of specific installer to be used to install package.
        /// e.g: nuget, folder, vsix(to download from VS marketplace), npm, maven...
        /// This is useful when installer can't be determined based on <see cref="InstallRequest.Identifier"/> and <see cref="InstallRequest.Details"/>
        /// </summary>
        string Name { get; }

        /// <summary>
        /// User can specify name of specific installer to be used to install package.
        /// e.g: nuget, folder, vsix(to download from VS marketplace), npm, maven...
        /// This is useful when installer can't be determined based on <see cref="InstallRequest.Identifier"/> and <see cref="InstallRequest.Details"/>
        /// </summary>
        Guid FactoryId { get; }

        Task<InstallResult> InstallAsync(InstallRequest installRequest);

        Task<UninstallResult> UninstallAsync(IManagedTemplatesSource managedSource);

        IManagedTemplatesSource Deserialize(IManagedTemplatesSourcesProvider provider, string mountPointUri, object details);

        (string mountPointUri, Dictionary<string, string> details) Serialize(IManagedTemplatesSource managedSource);

        Task<IReadOnlyList<IManagedTemplatesSourceUpdate>> GetLatestVersions(IEnumerable<IManagedTemplatesSource> sources);

        Task<IReadOnlyList<InstallResult>> UpdateAsync(IEnumerable<IManagedTemplatesSourceUpdate> sources);

        /// <summary>
        /// Creates list of autocomplete candidates for <see cref="InstallRequest.Identifier"/>.
        /// </summary>
        /// <param name="textSoFar">Text that was typed by user so far.</param>
        /// <param name="token">Calling code might call this on every key press, hence it is good idea to respect <see cref="CancellationToken"/></param>
        /// <returns>List of valid <see cref="InstallRequest.Identifier"/>s.</returns>
        Task<IReadOnlyList<string>> GetAutocompleteAsync(string textSoFar, CancellationToken token);
    }
}