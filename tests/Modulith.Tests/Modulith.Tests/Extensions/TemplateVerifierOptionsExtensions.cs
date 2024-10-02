namespace Modulith.Tests.Extensions;

public static class TemplateVerifierOptionsExtensions
{
  public static TemplateVerifierOptionsBuilder ForTemplate(string templateName) => new TemplateVerifierOptionsBuilder(templateName);
}
