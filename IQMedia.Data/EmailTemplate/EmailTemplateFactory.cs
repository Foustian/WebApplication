using System;
using System.Collections;
using IQMedia.Model;

namespace IQMedia.Data.EmailTemplate
{
    public class EmailTemplateFactory
    {
        private static readonly Hashtable _ETMap = new Hashtable();

        public static IEmailTemplate GetEmailTemplate(EmailTemplateType etType)
        {
            if (_ETMap[etType] == null)
                _ETMap[etType] = CreateEmailTemplate(etType);

            return (IEmailTemplate)_ETMap[etType];
        }

        private static IEmailTemplate CreateEmailTemplate(EmailTemplateType etType)
        {
            switch (etType)
            {
                case EmailTemplateType.EmailTemplate1:
                    return new EmailTemplate1DA();
                case EmailTemplateType.EmailTemplate2:
                    return new EmailTemplate2DA();
                case EmailTemplateType.EmailTemplateDevry:
                    return new EmailTemplateDevryDA();
                default:
                    //If we get to this point, no logic has been defined and the code 'SHOULD' fail...
                    throw new ArgumentException("No logic defined for requested type: '" + etType + "'");
            }
        }
    }

    public enum EmailTemplateType
    {
        EmailTemplate1,
        EmailTemplate2,
        EmailTemplateDevry
    }
}
