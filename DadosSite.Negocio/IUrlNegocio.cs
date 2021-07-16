using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace DadosSite.Negocio
{
    public interface IUrlNegocio
    {
        List<string> GetImagens(HtmlDocument html);

        Dictionary<string, int> RankingPalavras(HtmlDocument html, int ranking);
    }
}
