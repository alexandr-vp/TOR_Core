using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TOR_Core.CharacterDevelopment;
using TOR_Core.CharacterDevelopment.CareerSystem;

namespace TOR_Core.CampaignMechanics.ServeAsAHireling;

public class ServeAsAHirelingActivities
{
    private readonly Dictionary<CareerObject, List<SkillObject>> _activitySets;
    public ServeAsAHirelingActivities()
    {
        _activitySets = new Dictionary<CareerObject, List<SkillObject>>();
        _activitySets = new Dictionary<CareerObject, List<SkillObject>>()
        {
            //Mousillon
            {
                TORCareers.BlackGrailKnight, [
                    DefaultSkills.OneHanded,
                    DefaultSkills.Charm,
                    DefaultSkills.Riding,
                    DefaultSkills.Polearm,
                    DefaultSkills.Roguery
                ]
            },
            //Bretonnia
            {
                TORCareers.GrailKnight, [
                    DefaultSkills.OneHanded,
                    DefaultSkills.Charm,
                    DefaultSkills.Riding,
                    DefaultSkills.Polearm,
                    DefaultSkills.Roguery
                ]
            },
            {
                TORCareers.GrailDamsel, [
                    DefaultSkills.Riding,
                    TORSkills.SpellCraft,
                    TORSkills.Faith,
                    DefaultSkills.Steward,
                    DefaultSkills.Medicine
                ]
            },
            //Vampire Counts
            {
                TORCareers.Necromancer, [
                    DefaultSkills.Riding,
                    TORSkills.SpellCraft,
                    TORSkills.Faith,
                    DefaultSkills.Steward,
                    DefaultSkills.Medicine
                ]
            },
            {
                TORCareers.BloodKnight, [
                    DefaultSkills.OneHanded,
                    DefaultSkills.TwoHanded,
                    DefaultSkills.Riding,
                    DefaultSkills.Tactics,
                    DefaultSkills.Leadership
                ]
            },
            {
                TORCareers.MinorVampire, [
                    DefaultSkills.OneHanded,
                    DefaultSkills.Charm,
                    TORSkills.SpellCraft,
                    DefaultSkills.Roguery,
                    DefaultSkills.Leadership,
                ]
            },
            {
                TORCareers.Necrarch, [
                    TORSkills.SpellCraft,
                    DefaultSkills.Roguery,
                    DefaultSkills.Medicine,
                    DefaultSkills.Engineering,
                    DefaultSkills.Steward
                ]
            },
            //Empire
            {
                TORCareers.Mercenary, [
                    DefaultSkills.TwoHanded,
                    DefaultSkills.Bow,
                    TORSkills.GunPowder,
                    DefaultSkills.Trade,
                    DefaultSkills.Tactics,
                ]
            },
            {
                TORCareers.ImperialMagister, [
                    TORSkills.SpellCraft,
                    DefaultSkills.Steward,
                    DefaultSkills.OneHanded,
                    DefaultSkills.Medicine,
                    DefaultSkills.Tactics
                ]
            },
            {
                TORCareers.WitchHunter, [
                    DefaultSkills.TwoHanded,
                    DefaultSkills.Crossbow,
                    TORSkills.GunPowder,
                    TORSkills.Faith,
                    DefaultSkills.OneHanded
                ]
            },
            {
                TORCareers.WarriorPriest, [
                    DefaultSkills.OneHanded,
                    DefaultSkills.TwoHanded,
                    DefaultSkills.Athletics,
                    DefaultSkills.Medicine,
                    TORSkills.Faith,
                ]
            },
            {
                TORCareers.WarriorPriestUlric, [
                    DefaultSkills.Scouting,
                    DefaultSkills.TwoHanded,
                    DefaultSkills.Athletics,
                    DefaultSkills.Leadership,
                    TORSkills.Faith,
                ]
            },
            //Woodelves
            {
                TORCareers.Waywatcher, [
                    DefaultSkills.Bow,
                    DefaultSkills.Scouting,
                    DefaultSkills.Roguery,
                    DefaultSkills.Athletics,
                    DefaultSkills.Medicine
                ]
            },
            {
                TORCareers.Spellsinger, [
                    TORSkills.SpellCraft,
                    DefaultSkills.Riding,
                    TORSkills.Faith,
                    DefaultSkills.Charm,
                    DefaultSkills.Medicine
                ]
            },
            {
                TORCareers.Warden, [
                    DefaultSkills.OneHanded,
                    DefaultSkills.TwoHanded,
                    DefaultSkills.Leadership,
                    DefaultSkills.Scouting,
                    DefaultSkills.Medicine
                ]
            },
            {
                TORCareers.KnightOldWorld, [
                    DefaultSkills.Steward,
                    DefaultSkills.Charm,
                    DefaultSkills.Leadership,
                    DefaultSkills.Riding,
                    DefaultSkills.OneHanded,
                ]
            },
            //Eonir
            {
                TORCareers.GreyLord, [
                    TORSkills.SpellCraft,
                    DefaultSkills.Steward,
                    DefaultSkills.Leadership,
                    DefaultSkills.Charm,
                    DefaultSkills.Medicine
                ]
            },
            
            //Dwarfs
            
            {
                TORCareers.Ironbreaker, [
                    DefaultSkills.Steward,
                    DefaultSkills.Charm,
                    DefaultSkills.Leadership,
                    DefaultSkills.Riding,
                    DefaultSkills.OneHanded,
                ]
            },
            {
                TORCareers.Slayer, [
                    TORSkills.Faith,
                    DefaultSkills.Medicine,
                    DefaultSkills.Athletics,
                    DefaultSkills.TwoHanded,
                    DefaultSkills.OneHanded,
                ]
            },
            
        };


        foreach (var career in TORCareers.All)
        {
            if (!_activitySets.ContainsKey(career))
            {
                throw new Exception("Zerca register the hireling career acitivities, dumb ass! missing : "+career.Name);
            }
        }
        
        
        
    }
    
    public List<SkillObject> GetHirelingActivities(CareerObject careerObject)
    {
        if (_activitySets.TryGetValue(careerObject, out var activities))
        {
            return activities;
        }

        return new List<SkillObject>();
    }
}