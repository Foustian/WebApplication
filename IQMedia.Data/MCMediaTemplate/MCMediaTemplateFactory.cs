using System;
using System.Collections;
using IQMedia.Model;

namespace IQMedia.Data.MCMediaTemplate
{
    public class MCMediaTemplateFactory
    {
        private static readonly Hashtable _MTMap = new Hashtable();

        public static IMCMediaTemplate GetMCMediaTemplate(MCMediaTemplateType mtType)
        {
            if (_MTMap[mtType] == null)
                _MTMap[mtType] = CreateMCMediaTemplate(mtType);

            return (IMCMediaTemplate)_MTMap[mtType];
        }

        private static IMCMediaTemplate CreateMCMediaTemplate(MCMediaTemplateType mtType)
        {
            switch (mtType)
            {
                case MCMediaTemplateType.MCMediaTemplate1:
                    return new MCMediaTemplate1DA();
                case MCMediaTemplateType.MCMediaTemplate2:
                    return new MCMediaTemplate2DA();
                case MCMediaTemplateType.MCMediaTemplate3:
                    return new MCMediaTemplate3DA();
                case MCMediaTemplateType.MCMediaTemplateDemo:
                    return new MCMediaTemplateDemoDA();
                case MCMediaTemplateType.MCMediaTemplateTrivago:
                    return new MCMediaTemplateTrivagoDA();
                    break;
                default:
                    //If we get to this point, no logic has been defined and the code 'SHOULD' fail...
                    throw new ArgumentException("No logic defined for requested type: '" + mtType + "'");
            }
        }
    }

    public enum MCMediaTemplateType
    {
        MCMediaTemplate1,
        MCMediaTemplate2,
        MCMediaTemplate3,
        MCMediaTemplateDemo,
        MCMediaTemplateTrivago
    }
}
