using HtmlAgilityPack;
using System.Collections.Generic;

namespace DadosSite.Dominio
{
    public class UrlNegocio
    {
        private static List<string> GetImagens(HtmlDocument html)
        {
            string[] tags = { "img", "amp-img" };
            string[] atributos = { "src", "srcset" };
            var imagens = new List<string>();

            foreach (var tag in tags)
            {
                foreach (var atributo in atributos)
                {
                    var src = html.DocumentNode.Descendants(tag)
                                            .Select(e => e.GetAttributeValue(atributo, null))
                                            .Where(s => !String.IsNullOrEmpty(s));

                    foreach (var item in src)
                    {
                        if (item != null)
                        {
                            imagens.Add(item);
                        }
                    }
                }
            }

            return imagens;
        }

        private static Dictionary<string, int> RankingPalavras(HtmlDocument html, int ranking)
        {
            var htmlNodes = html.DocumentNode.SelectNodes("//body");
            var texto = "";

            foreach (var node in htmlNodes)
            {
                texto = string.Join(" ", node.InnerText);
            }

            List<string> palavras, artigo, pronome;
            texto = RemoverCaracteresEPalavrasEspecificas(texto, out palavras, out artigo, out pronome);

            var counts = palavras
                            .GroupBy(p => p.ToLower())
                            .Where(p => !artigo.Any(a => p.Contains(a)) && !pronome.Any(a => p.Contains(a)))
                            .OrderByDescending(f => f.Count())
                            .Take(ranking)
                            .ToDictionary(d => d.Key, d => d.Count());

            return counts;
        }

        private static string RemoverCaracteresEPalavrasEspecificas(string texto, out List<string> palavras, out List<string> artigo, out List<string> pronome)
        {
            texto = Regex.Replace(texto, @"<[^>]+>|&nbsp;", "").Trim();
            texto = Regex.Replace(texto, @"\s{2,}", " ");

            palavras = texto.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            artigo = "a,o,as,os,e,é,um,uma,uns,umas".Split(',').ToList();
            pronome = "no,na,em,que,com,de,do,da,qual,quais,este,esse,aquele,esta,essa,aquela,estes,esses,aqueles,estas,essas,aquelas".Split(',').ToList();

            return texto;
        }
    }
}
