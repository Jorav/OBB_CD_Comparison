using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OBB_CD_Comparison.src.old;


namespace OBB_CD_Comparison.src
{
    public class EntityFactory
    {

        public static int ScreenWidth;
        public static int ScreenHeight;

        public static string[] ReadConfig()
        {
            string filePath = Directory.GetCurrentDirectory();
            string conffile = Path.GetFullPath(Path.Combine(filePath, @"..\..\..\config.txt"));
            using (StreamReader sr = new StreamReader(conffile))
            {
                string[] lines = File.ReadAllLines(conffile);
                return lines;
            }
        }
        public static List<WorldEntity> EntFacImplementation(string SeedVar, string NbrObj, Texture2D textureParticle)
        {

            //Controller Entitycontroller = new Controller();
            //Controller Entitycontroller = controller;
            List<WorldEntity> EntityFacList = new List<WorldEntity>();

            int flSeed = int.Parse(SeedVar);
            int flNbrobj = int.Parse(NbrObj);



            Random rnd = new Random(flSeed);

            float rndrang1;
            float rndrang2;

            for (int j = 0; j < flNbrobj; j++)
            {
                rndrang1 = rnd.Next(-2000, 2000);
                rndrang2 = rnd.Next(-2000, 2000);
                //controllerlist.Add((Entitycontroller.AddEntity(new WorldEntity(textureParticle, new Vector2(rndrang1, rndrang2), 100f)));
                EntityFacList.Add(new WorldEntity(textureParticle, new Vector2(rndrang1, rndrang2), 100f));
            }
            return EntityFacList;
        }
    }
}
