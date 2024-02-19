using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SOMCHAIS_Adventure;

public class Camera
{
    public Matrix Transform { get; private set; }
    private float zoom = 1.5f; // Default zoom is 1 (no zoom)
    private int SCREENWIDTH = 800;
    private int SCREENHIGHT = 800;

    public float Zoom
    {
        get { return zoom; }
        set { zoom = MathHelper.Max(value, 0.1f); } // Prevent zoom from being less than 0.1f to avoid inversion
    }

    //public void Update(Vector2 position, int viewportWidth, int viewportHeight)
    //{
    //    // Adjust the position to the center of the screen
    //    var positionOffset = Matrix.CreateTranslation(
    //        -position.X + (viewportWidth / 2),
    //        -position.Y + (viewportHeight / 2),
    //        0);

    //    // Apply zoom
    //    var zoomMatrix = Matrix.CreateScale(new Vector3(zoom, zoom, 1));

    //    // Combine translation and zoom
    //    Transform = positionOffset * zoomMatrix;
    //}

    public void Update(Vector2 position, int viewportWidth, int viewportHeight)
    {
        // Calculate the camera's target position based on the player's position
        float targetX = position.X;
        float targetY = position.Y;

        // Calculate the edges of the camera view
        float leftBarrier = viewportWidth / 2f / zoom;
        float rightBarrier = SCREENWIDTH - viewportWidth / 2f / zoom;
        float topBarrier = viewportHeight / 2f / zoom;
        float bottomBarrier = SCREENHIGHT - viewportHeight / 2f / zoom;

        // Clamp the target position to ensure the camera stays within the world bounds
        targetX = MathHelper.Clamp(targetX, leftBarrier, rightBarrier);
        targetY = MathHelper.Clamp(targetY, topBarrier, bottomBarrier);

        // Adjust the position to the center of the screen, now considering clamping
        var positionOffset = Matrix.CreateTranslation(
            -targetX + (viewportWidth / 2),
            -targetY + (viewportHeight / 2),
            0);

        // Apply zoom
        var zoomMatrix = Matrix.CreateScale(new Vector3(zoom, zoom, 1));

        // Combine translation and zoom
        Transform = positionOffset * zoomMatrix;
    }

}
