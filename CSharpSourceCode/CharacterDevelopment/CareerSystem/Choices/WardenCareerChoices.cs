using System.Collections.Generic;
using TaleWorlds.Core;
using TOR_Core.CampaignMechanics.Choices;

namespace TOR_Core.CharacterDevelopment.CareerSystem.Choices;

public class WardenCareerChoices(CareerObject id) : TORCareerChoicesBase(id)
{
    private CareerChoiceObject _wardenRoot;
    protected override void RegisterAll()
    {
        _wardenRoot = Game.Current.ObjectManager.RegisterPresumedObject(new CareerChoiceObject("WardenRoot"));
    }

    protected override void InitializeKeyStones()
    {
        _wardenRoot.Initialize(CareerID,
            "{=black_grail_knight_root_str}The knight prepares a devastating charge, mounted or on foot, for the next 6 seconds. When mounted, the knight receives perk buffs as well as a 20% chance of his lance not bouncing off after a couched lance attack. The couched lance attack chance increases by 0.1% for every point in Riding. When on foot, the knight only receives perk buffs.",
            null,
            true, ChoiceType.Keystone, new List<CareerChoiceObject.MutationObject>()
            {

            });
    }

    protected override void InitializePassives()
    {
        
    }
}