using System.ComponentModel;

namespace PBT.DowsingMachine.Projects;

public class StringSelectConverter : StringConverter
{
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        var standardValuesAttribute = context.PropertyDescriptor.Attributes.OfType<SelectAttribute>().FirstOrDefault();
        return new StandardValuesCollection(standardValuesAttribute?.Options);
    }
}
