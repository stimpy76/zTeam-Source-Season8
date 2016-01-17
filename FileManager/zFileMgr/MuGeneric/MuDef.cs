using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace zFileMgr.MuGeneric
{
    class MuDef
    {
        #region Generic for all bmd files

        public const ushort cBmdVersion = 21575;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TBmdHead
        {
            public ushort Version;
            public int LineCount;
        }

        #endregion


       public enum ItemType : byte
        {
            Swords,
            Axes,
            Maces,
            Spears,
            Bows,
            Staffs,
            Shields,
            Helms,
            Armors,
            Pants,
            Gloves,
            Boots,
            Wings,
            Special,
            Jewels,
            Scrolls
        }

        #region MasterSkillTooltip

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_MasterSkillTooltip
        {
            public int SkillID;
            public ushort Type;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Info1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Info2;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Info3;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Info4;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Info5;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 130)]
            public string Info6;

        }

        #endregion

        #region InfoTooltipText

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_InfoTooltipText
        {
            public ushort ID;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Text;
        }

        #endregion

        #region BuffEffect

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_BuffEffect
        {
            public ushort Index;
            public byte Group;
            public byte ItemIndex;
            public byte ItemNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string Name;
            public byte State1;
            public byte State2;
            public byte State3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string Description;
        }

        #endregion

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_ClientCommon
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string IP;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string Version;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
            public string Serial;
            public UInt16 Port;
            public UInt16 ChatEncoding;
            public byte ChatGlobalR;
            public byte ChatGlobalG;
            public byte ChatGlobalB;
        };

        #region Generic filter struct [Filter.bmd, FilterName.bmd]

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_GenericFilter
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string Text;
        };

        #endregion

        #region Gate

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_Gate
        {
            public byte Type;
            public byte MapNumber;
            public byte X1;
            public byte Y1;
            public byte X2;
            public byte Y2;
            public ushort Target;
            public ushort Dir;
            public ushort Level;
            public ushort MaxLevel;
        };

        #endregion

        #region Credit

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_Credit
        {
            public byte Position;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Text;
        };

        #endregion

        #region MoveReq

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_MoveReq
        {
            public int ID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Text1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Text2;

            public int MinLevel;
            public int MaxLevel;
            public int Money;
            public int Gate;
        };

        #endregion

        #region JewelOfHarmonyOption

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_JewelOfHarmonyOption
        {
            public int Index;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Name;
            public int Level1;
            public int Level2;
            public int Level3;
            public int Level4;
            public int Level5;
            public int Level6;
            public int Level7;
            public int Level8;
            public int Level9;
            public int Level10;
            public int Level11;
            public int Level12;
            public int Level13;
            public int Level14;

            public int Zen1;
            public int Zen2;
            public int Zen3;
            public int Zen4;
            public int Zen5;
            public int Zen6;
            public int Zen7;
            public int Zen8;
            public int Zen9;
            public int Zen10;
            public int Zen11;
            public int Zen12;
            public int Zen13;
            public int Zen14;
            
        }

        #endregion

        #region Text

        //probably will remove this because of unnecesarity [check MuBMD.Text]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_Text
        {
           // public int LineNr;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string Text;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_TextItemHead
        {
            public int LineNr;
            public int LineLen;
        }


        public struct MUFile_TextManaged
        {
            public int LineNr;
            public string Text;
        }

        #endregion

        #region NPCName

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_NPCName
        {
            public ushort ID;
            public ushort Type;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Name;
        }

        #endregion

        #region ServerList

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_ServerListItem
        {
            public ushort ServerID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Name;

            //3
            public byte Unknown1;
            public byte Unknown2;
            public byte Unknown3;

            //19 
            public byte Unknown4;
            public byte Unknown5;
            public byte Unknown6;
            public byte Unknown7;
            public byte Unknown8;
            public byte Unknown9;
            public byte Unknown10;
            public byte Unknown11;
            public byte Unknown12;
            public byte Unknown13;
            public byte Unknown14;
            public byte Unknown15;
            public byte Unknown16;
            public byte Unknown17;
            public byte Unknown18;
            public byte Unknown19;
            public byte Unknown20;
            public byte Unknown21;
            public byte Unknown22;
            public ushort DescLen;
        }

        public struct MuFile_ServerListManaged
        {
            public ushort ServerID;
            public string Name;
            public byte[] Unknown;
        }

        #endregion

        #region ItemTooltipText

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_ItemTooltipText
        {
            public ushort ID;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Text;

            public ushort Unknown;
        }

        #endregion

        #region Skill

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_Skill
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Name;

            public ushort Level;
            public ushort Damage;
            public ushort Mana;
            public ushort BP;

            public int Dis;
            public int Delay;
            public int Energy;
            public ushort Leadership;
            public byte Attr;
            public byte UseType;

            public int Brand;
            public byte KillCount;

            public byte KillStatus1;
            public byte KillStatus2;
            public byte KillStatus3;

            public byte Class1;
            public byte Class2;
            public byte Class3;
            public byte Class4;
            public byte Class5;
            public byte Class6;
            public byte Class7;

            public byte SkillRank;
            public ushort SkillGroup;

            public byte Unknown3;
            public byte Unknown4;

            public int ReqStr;
            public int ReqDex;

            public byte ItemSkill;
            public byte IsDamage;
            public ushort SkillIcon;

            public byte Unknown5;
            public byte Unknown6;
            public byte Unknown7;
            public byte Unknown8;
            public byte Unknown9;
            public byte Unknown10;
            public byte Unknown11;
            public byte Unknown12;


        }

        #endregion

        #region Item
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_ItemExcellentOption
        {
            public ushort ItemCode;
            public byte Option0;
            public byte Option1;
            public byte Option2;
            public byte Option3;
            public byte Option4;
            public byte Option5;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_ItemGlow
        {
            public ushort ItemCode;
            public byte R;
            public byte G;
            public byte B;
        }

        public struct MUFile_Item
        {
            public int ItemID;

            public ushort Index;
            public ushort Number;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string TexturePath;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string ModelName;

            public MUFile_ItemInfo ItemInfo;
        }

        public struct MUFile_ItemInfo
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
            public string Name;

            public byte Type1;
            public byte Type2;
            public byte Type3;

            public byte TwoHands;
            public ushort ItemLvl;
            public ushort ItemSlot;
            public ushort ItemSkill;
            public byte X;
            public byte Y;
            public ushort DmgMin;
            public ushort DmgMax;
            public ushort DefRate;
            public ushort Defence;
            public ushort Unknown1;

            public byte AtkSpeed;
            public byte WalkSpeed;
            public byte Durability;
            public byte MagicDur;
            public byte MagicPwr;
            public byte Unknown2;

            public ushort ReqStr;
            public ushort ReqDex;
            public ushort ReqEne;
            public ushort ReqVit;
            public ushort ReqLea;
            public ushort ReqLvl;
            public ushort ItemValue;

            public int Zen;
            public byte SetOption;

            
          

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] Classes;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] Resistances;

            public byte IsApplyToDrop;
            public byte Unknown3;
            public byte Unknown4;
            public byte Unknown5;
            public byte Unknown6;
            public byte IsExpensive;
            public byte Unknown7;
            public byte StackMax;

            //+0 = Is drop apply item
            //+5 = IsExpensive (sell notice, drop block)
            //+7 = Max durability (for potions, rena and etc.)


            

            public byte Pad;

        }

        #endregion

        #region NPCDialog

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_NPCDialog
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 58)]
            public byte[] Values;
        }

        #endregion

        #region Slide

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_Slide
        {
            public int Delay;
            public int SlideCount;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide4;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide5;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide6;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide7;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide8;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide9;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide10;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide11;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide12;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide13;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide14;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide15;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide16;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide17;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide18;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide19;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide21;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide22;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide23;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide24;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide25;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide26;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide27;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide28;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide29;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide30;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide31;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Slide32;

        }

        #endregion

        #region MasterSkillTreeData

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_MasterSkillTreeData
        {
            public byte MasterSkillID;
            public byte Tmp;
            public ushort Type;
            public byte Group;
            public byte RequiredPoints;
            public byte MaxLevel;
            public byte Unknown1;
            public int Skill1;
            public int Skill2;
            public int Skill3;
            public float DefValue;
        }

        #endregion

        #region MonsterSkill

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_MonsterSkill
        {
            public int MonsterID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public int[] MonsterSkill;
            public int Unknown1;
        }
        #endregion

        #region ItemLevelTooltip

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_ItemLevelTooltip
        {
            public short ID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Name;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Unknown1;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public short[] Unknown2;
        }

        #endregion

        #region QuestWord

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_QuestWordHead
        {
            public int ID;
            public ushort Length;
        }


        public struct MUFile_QuestWord_NoMarshal
        {
            public int ID;
            public string Word;
        }
        #endregion

        #region HelpData

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_HelpData
        {
            public int ID;
            public byte bt1;
            public byte bt2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Text1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Text2;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
            public string Text3;
        }

        #endregion

        #region QuestProgress

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_QuestProgress
        {
            public ushort ID1;
            public ushort ID2;
            public byte ID3;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            public ushort[] ID4;
        }

        #endregion

        #region Minimap

        //ok both s8/previous ver
        public struct MUFile_Minimap
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public int[] Params;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string Name;

        }

        public struct MUFile_Minimap_NoMarshal
        {
            public int MapIndex;
            public MUFile_Minimap MinimapStruct;
        }

        #endregion

        #region ItemAddOption

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_ItemAddOption
        {
            public ushort ItemOpt1;
            public ushort ItemVal1;
            public ushort ItemOpt2;
            public ushort ItemVal2;
            public int Index;
            public int Time;
        }

        #endregion

        #region ItemSetType
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_ItemSetType
        {
            public byte Set1;
            public byte Set2;
            public byte Level1;
            public byte Level2;
        }
        #endregion

        #region ItemTRSData
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_ItemTRSData
        {
            public int ItemCode;
            public float TranslationX;
            public float TranslationY;
            public float TranslationZ;
            public float Rotation;
            public float ScaleX;
            public float ScaleY;
            public float ScaleZ;
        }
        #endregion

        #region ItemSetOptionText
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_ItemSetOptionText
        {
            public byte Index;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
            public string Text;
        }
        #endregion

        #region ItemSetOption
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_ItemSetOption
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	        public sbyte[] OptionTable0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	        public sbyte[] OptionTable1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	        public sbyte[] OptionTableValue0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	        public sbyte[] OptionTableValue1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public sbyte[] ExOptionTable;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	        public sbyte[] ExOptionTableValue;
            public sbyte FullOptionCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	        public sbyte[] FullOptionTable;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	        public sbyte[] FullOptionTableValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
	        public sbyte[] ReqClassTable;
        }
        #endregion

        /*------------------------------------------------------------*/
        /* Unfinished / buggy */

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_PetData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            public byte[] Values;

        }

        


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MUFile_SocketItem
        {
            public int Index;
            public int Type;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Name;

            public int Option1;
            public int Option2;
            public int Option3;
            public int Option4;
            public int Option5;
            public int Option6;

            public byte SetOption1;
            public byte SetOption2;
            public byte SetOption3;
            public byte SetOption4;
            public byte SetOption5;
            public byte SetOption6;
    
          //  public byte SetOption7;

        }




        /*------------------------------------------------------------*/
    }
}
