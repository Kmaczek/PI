using Core.Domain.Logic.EmailGeneration;
using System;

namespace dkapp
{
    internal class EmailAssembler
    {
        private System.Collections.Generic.List<IHtmlGenerator> list;

        public EmailAssembler(System.Collections.Generic.List<IHtmlGenerator> list)
        {
            this.list = list;
        }

        internal string GenerateEmail()
        {
            throw new NotImplementedException();
        }
    }
}