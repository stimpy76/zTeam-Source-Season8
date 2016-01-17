[08.05.2014]
- Fix replace single / all items (ClientEditor.cs)
- Some code refactoring at MuFile.cs and few other classes
- Test Gate.bmd editing
- Other file formats support to add

readme 
[10.05.2014 02:51]
- Most of stuff re-factored 
	- Deleted MuFile.cs (now using abstract class - BmdFile)
	- Moved mu file structure definitions to MuDef.cs
	[needs some testing]
	- BmdFile.cs
	- BuffEffect.cs 
	- Credits.cs 
	- GenericFilter.cs 
	[/needs some testing]
- GUI performance improvements (RowAdded and RowRemoved event loop at ClientEditor)
- Fast search feature added (enter a search term and press enter)
- Fast go to line feature added (enter line number and press enter)

readme
[11.05.2014 04:21]
//--------------------------------------------------------
- Control + Z now has action stack
- Added cancel button to close file dialog
- Added save last folder path to open file [user config scope]
- Added save last folder path to save file [user connfig scope]
- Fixed search (should be OK now)
- Added Item -> JewelOfHarmonyOption.bmd  (JewelOfHarmonyOption.cs)
- Added Gate.bmd (Gate.cs)
- Added MoveReq.bmd -> (MoveReq.cs)
- Added Text.bmd -> (Text.cs)
- Added ServerList.bmd 
	- Not sure about description stuff (encoding etc)
	- No save method for now
- Added TBmdHeader to MuDef.cs

- Implement conversation to txt file 
- ServerList.bmd -> write method
- Prevent IndexOfOfRange thrown at derived classes of BmdFile
//--------------------------------------------------------

[12.05.2014 04:40]
readme
- Added NPCName (NPCName.cs)
- Added InfoTooltipText (InfoTooltipText.cs)
- Added MasterSkillTooltip (MasterSkillTooltip.cs) [ needs more testing ]
- Added ItemTooltipText (ItemTooltipText.cs)

- ServerList.cs -> fixed reading / saving
todo
- SocketItem.cs -> fix structure and all the stuff
- Add bmd -> text methods to BmdFile 
- Add text -> bmd methods to BmdFile

readme
	[test]
- Export BuffEffect to txt -> done
- Export Filter to txt -> done
- Export FilterName to txt -> done
- Export InfoTooltipText to txt -> done
- Export NPCName to txt -> done
- Export Text to txt -> done
- Export MasterSkillTooltip to txt -> done
- Export Gate to xml -> done
	[/test]


- Export Gate to txt -> done
- Export MoveReq to xml -> done
- wrapped xml / txt saving into try/catch blocks

- Skill.bmd <-> bmd -> done
- Reading / grouping item.bmd -> done

todo:
- Saving item.bmd [will be some pretty huge butthurt]
- Importing from xml/txts if needed
- Exporting skill.bmd -> txt / xml if needed


- Done

	- ClientEditor
		- Replace / Replace all (CTRL + G)
		- Search (CTRL + F)
		- BuffEffect - bmd, xml, text
		- Credits - bmd, xml, text
		- Gate - bmd, xml, text
		- NameFilter - bmd, xml, text
		- Filter - bmd, xml, text
		- InfoTooltipText - bmd, xml, text
		- Item - bmd
		- ItemTooltipText - bmd, xml, text
		- JewelOfHarmonyOption - bmd, xml, text
		- MasterSkillTooltip - bmd, xml, text
		- Minimap - bmd
		- MoveReq - bmd, xml, text
		- NPCName - bmd, xml, text
		- ServerList - bmd, text
		- Skill - bmd, xml, text
		- Text - bmd, xml, text



//---------------------------------------------------
23.06.2014

- Added
	- Highlighting newly inserted row

- TEST
	- NPCDialog.bmd - bmd
	- Slide.bmd - bmd
	- MasterSkillTreeData.bmd - bmd [check TODO]
	- MonsterSkill.bmd - bmd
	- ItemLevelTooltip.bmd - bmd
	- QuestWords.bmd - bmd
	- QuestProgress.bmd - bmd



24.06.2014
- TEST
	- HelpData.bmd - bmd
//---------------------------------------------------

- TODO
	- MasterSkillTreeData CRC key required
	- Add another version of BMD header (8 bytes long one)
	- QuestProgress.bmd s8 structure required
	- PetData.bmd s8 structure required
	- Message(eng).wtf structure required

02.07.2014
- TEST
	- MasterSkillTreedata CRC added (works???)

- Obfuscate all release builds from now

13.07.2014
	- TEST
		- Minimap.bmd - bmd
		- ItemSetType.bmd - bmd
		- ItemSetOption.bmd - bmd