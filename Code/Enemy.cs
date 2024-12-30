using System.Runtime.Versioning;
using Sandbox;
using Sandbox.ModelEditor.Nodes;
using Common.Utilities;

public sealed class Enemy : Component 
{
    private List<GameObject> players;
    private List<GameObject> player_bits;

    [Property] private SkinnedModelRenderer modelRenderer { get; set; }

    [Property] private int weapon = 6; // 6 is SWORD

    [Property] private int move_speed = 50;

    private Rigidbody rb;

    protected override void OnStart()
    {
        rb = GameObject.GetComponent<Rigidbody>();

        modelRenderer = GameObject.GetOrAddComponent<SkinnedModelRenderer>();
        modelRenderer.Model = Model.Load("models/citizen/citizen.vmdl");
        modelRenderer.UseAnimGraph = true;

        modelRenderer.Set("b_grounded", true);
        modelRenderer.Set("holdtype", weapon);
        

        // print each animation name
        foreach (var anim in modelRenderer.Model.AnimationNames)
        {
            Log.Info(anim);
        }

        
        // Find all players based on tag (only parent objects, no children)
        players = FindParentsByTag("player");

        // Find all objects with the "player" tag, including children
        player_bits = FindGameObjectsByTag("player");

        // Log the found players
        if (players.Count > 0)
        {
            Log.Info($"Found {players.Count} players. and {player_bits.Count} player bits");
            foreach (var bit in player_bits)
            {
                Log.Info($"Player: {bit}");
            }
        }
        else
        {
            Log.Info("No players found.");
        }
    }

protected override void OnUpdate()
{
    modelRenderer.Set("holdtype", weapon);
    
    // Go towards player
    if (players.Count > 0)
    {
        var player = players[0];
        var direction = player.WorldPosition - GameObject.WorldPosition;

        // Rotate to face the player
        GameObject.LocalRotation = Rotation.LookAt(direction.Normal);
        if (direction.Length <= 100) direction = 0;
        rb.SmoothMove(GameObject.WorldPosition + direction.Normal * move_speed, 1, Time.Delta);


        // Set movement animation
        modelRenderer.Set("move_x", direction.Normal.x * move_speed);
        modelRenderer.Set("move_y", direction.Normal.y * move_speed);
        modelRenderer.Set("move_z", direction.Normal.z * move_speed);

        if (direction == Vector3.Zero)
            modelRenderer.Set("duck", 0.4f);
        else
            modelRenderer.Set("duck", 0.0f);
    }
}


    private List<GameObject> FindGameObjectsByTag(string tag) 
    {
        return Scene.GetAllObjects(true)
                    .Where(go => go.Tags.Has(tag))
                    .ToList();
    }

    private List<GameObject> FindParentsByTag(string tag) 
    {
        return FindGameObjectsByTag(tag)
                    .Where(go => go.Parent.Tags.Has("player") == false)
                    .ToList();
    }

}


