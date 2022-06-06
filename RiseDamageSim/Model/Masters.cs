using RiseDamageSim.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RiseDamageSim.Model
{
    static internal class Masters
    {
        private static readonly JsonSerializerOptions options = new();

        public static List<Monster> Monsters;
        public static List<Weapon> GreatSwords;
        public static List<Motion> GreatSwordMotion;
        public static List<CompareSet> CompareSets;
        public static List<string> RampageSkills;

        static Masters()
        {
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All);
            options.Converters.Add(new JsonStringEnumConverter());

            LoadDatas();
        }

        private static void LoadDatas()
        {
            LoadWeaponDatas();
            SetRampageSkillsData();
            LoadMonsterDatas();
            LoadCompareSets();
        }

        private static void SetRampageSkillsData()
        {
            RampageSkills = new();
            foreach (var weapon in GreatSwords)
            {
                foreach (var weaponRampage in weapon.RampageSkills)
                {
                    bool isExist = false;
                    foreach (var rampage in RampageSkills)
                    {
                        if (rampage == weaponRampage)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        RampageSkills.Add(weaponRampage);
                    }
                }
            }
            RampageSkills.Sort();
        }

        private static void LoadWeaponDatas()
        {
            LoadGreatSwordDatas();
        }

        private static void LoadGreatSwordDatas()
        {
            string json = File.ReadAllText("data/GreatSwords.json");
            List<Weapon>? greatSwords = JsonSerializer.Deserialize<List<Weapon>>(json, options);
            if (greatSwords == null)
            {
                throw new FileFormatException("data/GreatSwords.json");
            }
            GreatSwords = greatSwords;

            string json2 = File.ReadAllText("data/GreatSwordMotion.json");
            List<Motion>? greatSwordMotion = JsonSerializer.Deserialize<List<Motion>>(json2, options);
            if (greatSwordMotion == null)
            {
                throw new FileFormatException("data/GreatSwordMotion.json");
            }
            GreatSwordMotion = greatSwordMotion;
        }

        private static void LoadMonsterDatas()
        {
            string json = File.ReadAllText("data/Monsters.json");
            List<Monster>? monsters = JsonSerializer.Deserialize<List<Monster>>(json, options);
            if (monsters == null)
            {
                throw new FileFormatException("data/Monsters.json");
            }
            Monsters = monsters;
        }

        private static void LoadCompareSets()
        {
            string json = File.ReadAllText("save/CompareSets.json");
            List<CompareSet>? sets = JsonSerializer.Deserialize<List<CompareSet>>(json, options);
            if (sets == null)
            {
                throw new FileFormatException("save/CompareSets.json");
            }
            CompareSets = sets;
        }

        public static void SaveCompareSets()
        {
            string json = JsonSerializer.Serialize(CompareSets, options);
            File.WriteAllText("save/CompareSets.json", json);
        }

        public static Weapon? GetWeapon(string name)
        {
            foreach (var weapon in GreatSwords)
            {
                if (weapon.Name == name)
                {
                    return weapon;
                }
            }
            return null;
        }

        public static Equipment? GetEquipment(string id)
        {
            foreach (var set in CompareSets)
            {
                foreach (var equip in set.Equipments)
                {
                    if (equip.Id == id)
                    {
                        return equip;
                    }
                }
            }
            return null;
        }

        internal static void AddEquipment(CompareSet set, Equipment equip)
        {
            set.Equipments.Add(equip);
        }


        internal static void DeleteEquipment(CompareSet set, Equipment equip)
        {
            set.Equipments.Remove(equip);
            SaveCompareSets();
        }

        internal static CompareSet? GetSet(CompareSet compareSet)
        {
            foreach (var set in CompareSets)
            {
                if (compareSet == set)
                {
                    return set;
                }
            }
            return null;
        }

        internal static void AddSet(CompareSet set)
        {
            CompareSets.Add(set);
        }

        internal static bool DeleteSet(CompareSet set)
        {
            bool isDeleted = CompareSets.Remove(set);
            SaveCompareSets();
            return isDeleted;
        }
    }
}
