namespace VirtualPetApp.Models;

public sealed class Pet
{
    public string Name { get; set; } = "Pet";
    public PetType Type { get; set; } = PetType.Dog;

    // Stats 0..100
    public int Hunger { get; set; } = 30;      // lower is better
    public int Energy { get; set; } = 70;
    public int Cleanliness { get; set; } = 70;
    public int Happiness { get; set; } = 70;
    public int Health { get; set; } = 80;

    public Mood Mood { get; set; } = Mood.Okay;

    public static Pet CreateDefault(PetType type, string name)
    {
        var p = new Pet { Type = type, Name = name };

        // Slightly different starting stats per type
        switch (type)
        {
            case PetType.Dog:
                p.Energy = 75; p.Happiness = 75; p.Cleanliness = 65; p.Health = 80; p.Hunger = 35;
                break;
            case PetType.Cat:
                p.Energy = 65; p.Happiness = 70; p.Cleanliness = 75; p.Health = 78; p.Hunger = 32;
                break;
            case PetType.Dragon:
                p.Energy = 80; p.Happiness = 60; p.Cleanliness = 55; p.Health = 85; p.Hunger = 40;
                break;
            case PetType.Hamster:
                p.Energy = 60; p.Happiness = 72; p.Cleanliness = 70; p.Health = 76; p.Hunger = 30;
                break;
        }

        p.ClampAll();
        p.UpdateMood();
        return p;
    }

    public void ClampAll()
    {
        Hunger = Math.Clamp(Hunger, 0, 100);
        Energy = Math.Clamp(Energy, 0, 100);
        Cleanliness = Math.Clamp(Cleanliness, 0, 100);
        Happiness = Math.Clamp(Happiness, 0, 100);
        Health = Math.Clamp(Health, 0, 100);
    }

    public void UpdateMood()
    {
        // Simple mood rules
        if (Health < 30) { Mood = Mood.Sick; return; }

        int score = 0;
        score += (100 - Hunger) / 5;      // hunger lower = better
        score += Energy / 5;
        score += Cleanliness / 5;
        score += Happiness / 5;
        score += Health / 5;

        // score ranges roughly 0..100
        if (score >= 70) Mood = Mood.Happy;
        else if (score >= 45) Mood = Mood.Okay;
        else Mood = Mood.Sad;
    }
}
