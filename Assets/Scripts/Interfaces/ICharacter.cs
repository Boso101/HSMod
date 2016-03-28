﻿public interface ICharacter
{
    int BaseHealth { get; set; }
    int CurrentHealth { get; set; }

    int BaseAttack { get; set; }
    int CurrentAttack { get; set; }

    void Attack(ICharacter target);
    void Damage(int damageAmount);
}