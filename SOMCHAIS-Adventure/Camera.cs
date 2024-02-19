using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SOMCHAIS_Adventure;

public class Camera
{
    public Matrix Transform { get; private set; }
    private float zoom = 1.5f; // Default zoom is 1 (no zoom)

    public float Zoom
    {
        get { return zoom; }
        set { zoom = MathHelper.Max(value, 0.1f); } // Prevent zoom from being less than 0.1f to avoid inversion
    }

    public void Update(Vector2 position, int viewportWidth, int viewportHeight)
    {
        // Adjust the position to the center of the screen
        var positionOffset = Matrix.CreateTranslation(
            -position.X + (viewportWidth / 2),
            -position.Y + (viewportHeight / 2),
            0);

        // Apply zoom
        var zoomMatrix = Matrix.CreateScale(new Vector3(zoom, zoom, 1));

        // Combine translation and zoom
        Transform = positionOffset * zoomMatrix;
    }
}
