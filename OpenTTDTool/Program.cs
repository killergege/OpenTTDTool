﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net.Config;

namespace OpenTTDTool
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var parsedInfos = new List<NfoRowParser>();
            //TODO  : Vérifier que l'encoding est toujours 437 (OEM-US)
            using (var file = new StreamReader(@"..\..\..\GrfExampleFiles\proj1000.nfo", Encoding.GetEncoding(Constants.CODE_PAGE_NFO)))
            //using (var file = new StreamReader(@"D:\Projets\GRF\2cc_TrainsInNML-1.0\sprites\2ccts.nfo"))
            {
                while (!file.EndOfStream)
                {
                    var ligne = file.ReadLine();

                    if (ligne.Replace(" ", "").StartsWith("//") || String.IsNullOrWhiteSpace(ligne))
                        continue;

                    if (ligne.StartsWith("\t"))
                        parsedInfos.Last().AddContent(ligne);
                    else
                        parsedInfos.Add(new NfoRowParser(ligne));
                }
                parsedInfos.ForEach(p => p.Parse());

                //var lister = parsedInfos.Where(p => p.Action == (int)Actions.Labels).ToList();
                ////lister.ForEach(p => { var nom = p.ParsedText[7]; Console.WriteLine(String.Format("{0} - {1} - {2}", p.Number, (int)nom[0], nom.Substring(1))); });
                //lister.ForEach(p => Console.WriteLine(p.FullText));

                VehicleManager.Instance.Vehicles.Select(p => p.Value).ToList().ForEach(p => { Console.WriteLine(p.ToString()); Console.ReadKey(); });
                //VehicleManager.Instance.Vehicles.Select(p => p.Value).Where(p=>String.IsNullOrWhiteSpace( p.Name)) .ToList().ForEach(p => { Console.WriteLine(p.ToString());/* Console.ReadKey();*/ });


                Console.ReadKey();
            }
        }
    }
}
