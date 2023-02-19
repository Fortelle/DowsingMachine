using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace PBT.DowsingMachine.Projects;

public class EnumSelectConverter : EnumConverter
{
    public EnumSelectConverter([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type) : base(type)
    {
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        var standardValuesAttribute = context.PropertyDescriptor.Attributes.OfType<SelectAttribute>().FirstOrDefault();
        return new StandardValuesCollection(standardValuesAttribute?.Options);
    }
}
