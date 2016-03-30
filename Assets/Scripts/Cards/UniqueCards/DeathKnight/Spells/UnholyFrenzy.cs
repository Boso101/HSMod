public class UnholyFrenzy : SpellCard
{
    public UnholyFrenzy()
    {
        Name = "Unholy Frenzy";
        Description = "Deal 1 damage to a minion. Give it +4 Attack";

        CardClass = CardClass.DeathKnight;
        Rarity = Rarity.Common;

        TargetType = TargetType.TargetAllMinions;

        Cost = 1;
    }

    public override void Cast(ICharacter target)
    {
        //EventManager.OnSpellPreCast(this);

        target.Damage(null, 1);
        // TODO : Add +4 attack

        //EventManager.OnSpellCasted(this);
    }
}