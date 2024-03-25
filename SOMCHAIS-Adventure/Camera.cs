using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SOMCHAIS_Adventure;

public class Camera
{
    public Matrix Transform { get; private set; }
    private float zoom = 1f; // Default zoom is 1 (no zoom)
    private int SCREENWIDTH = Singleton.SCREENWIDTH;
    private int SCREENHIGHT = Singleton.SCREENHEIGHT;

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


    // core function for zoom at player [1]
    public void Update(Vector2 position, int viewportWidth, int viewportHeight)
    {
        // Apply zoom first
        var zoomMatrix = Matrix.CreateScale(new Vector3(zoom, zoom, 1));

        // Calculate the zoom adjusted position to keep the player centered
        Vector2 zoomAdjustedPosition = position * zoom;

        // Adjust the position to the center of the screen, considering the zoom
        var positionOffset = Matrix.CreateTranslation(
            -zoomAdjustedPosition.X + (viewportWidth / 2f),
            -zoomAdjustedPosition.Y + (viewportHeight / 2f),
            0);

        // Combine zoom and translation
        Transform = zoomMatrix * positionOffset;
    }

    //// new [2]
    //public void Update(Vector2 position, int viewportWidth, int viewportHeight)
    //{
    //    // Calculate the edges of the camera view after applying zoom
    //    float viewWidth = viewportWidth / zoom;
    //    float viewHeight = viewportHeight / zoom;

    //    // Calculate the minimum and maximum x and y values the camera can be at
    //    float minX = viewWidth / 2;
    //    float maxX = 800 - (viewWidth / 2);
    //    float minY = viewHeight / 2;
    //    float maxY = 600 - (viewHeight / 2);

    //    // Clamp the camera position to ensure it doesn't go outside the world bounds
    //    Vector2 clampedPosition = Vector2.Clamp(position, new Vector2(minX, minY), new Vector2(maxX, maxY));

    //    // Apply the clamped position with the adjustments for zoom and translation
    //    var positionOffset = Matrix.CreateTranslation(
    //        -clampedPosition.X + (viewportWidth / 2),
    //        -clampedPosition.Y + (viewportHeight / 2),
    //        0);
    //    var zoomMatrix = Matrix.CreateScale(new Vector3(zoom, zoom, 1));
    //    Transform = zoomMatrix * positionOffset;
    //}

    // [3] not solve
    //public void Update(Vector2 position, int viewportWidth, int viewportHeight)
    //{
    //    // Adjust the viewport size based on the current zoom level to calculate the view boundaries correctly
    //    float adjustedViewportWidth = viewportWidth / zoom;
    //    float adjustedViewportHeight = viewportHeight / zoom;

    //    // Calculate the edges of the camera view, considering the adjusted viewport size
    //    float minX = adjustedViewportWidth / 2;
    //    float maxX = 800 - minX;
    //    float minY = adjustedViewportHeight / 2;
    //    float maxY = 600 - minY;

    //    // Clamp the camera position to ensure it doesn't show areas outside of the world bounds
    //    Vector2 clampedPosition = Vector2.Clamp(position, new Vector2(minX, minY), new Vector2(maxX, maxY));

    //    // Center the camera on the clamped position, adjusting for the zoom level
    //    var positionOffset = Matrix.CreateTranslation(
    //        -clampedPosition.X + (viewportWidth / 2),
    //        -clampedPosition.Y + (viewportHeight / 2),
    //        0);

    //    // Apply zoom
    //    var zoomMatrix = Matrix.CreateScale(new Vector3(zoom, zoom, 1));

    //    // Combine the zoom and position offset to get the final transform
    //    Transform = zoomMatrix * positionOffset;
    //}

    // [4]
    //public void Update(Vector2 position, int viewportWidth, int viewportHeight)
    //{
    //    // Calculate the effective viewport size considering the current zoom level
    //    float effectiveViewportWidth = viewportWidth / zoom;
    //    float effectiveViewportHeight = viewportHeight / zoom;

    //    // Calculate the camera boundaries to ensure the camera does not move outside the world
    //    float minX = effectiveViewportWidth / 2f;
    //    float maxX = 800 - effectiveViewportWidth / 2f;
    //    float minY = effectiveViewportHeight / 2f;
    //    float maxY = 600 - effectiveViewportHeight / 2f;

    //    // Clamp the position to these boundaries
    //    Vector2 clampedPosition = new Vector2(
    //        MathHelper.Clamp(position.X, minX, maxX),
    //        MathHelper.Clamp(position.Y, minY, maxY));

    //    // Adjust the camera to focus on the clamped position, considering zoom
    //    Matrix positionOffset = Matrix.CreateTranslation(
    //        -clampedPosition.X + viewportWidth / 2f,
    //        -clampedPosition.Y + viewportHeight / 2f,
    //        0);

    //    // Apply zoom
    //    Matrix zoomMatrix = Matrix.CreateScale(new Vector3(zoom, zoom, 1));

    //    // Combine the effects of zoom and translation
    //    Transform = zoomMatrix * positionOffset;
    //}



    //public void Update(Vector2 position, int viewportWidth, int viewportHeight)
    //{
    //    // Calculate the camera's target position based on the player's position
    //    float targetX = position.X;
    //    float targetY = position.Y;

    //    // Calculate the edges of the camera view
    //    float leftBarrier = viewportWidth / 2f / zoom;
    //    float rightBarrier = SCREENWIDTH - viewportWidth / 2f / zoom;
    //    float topBarrier = viewportHeight / 2f / zoom;
    //    float bottomBarrier = SCREENHIGHT - viewportHeight / 2f / zoom;

    //    // Clamp the target position to ensure the camera stays within the world bounds
    //    targetX = MathHelper.Clamp(targetX, leftBarrier, rightBarrier);
    //    targetY = MathHelper.Clamp(targetY, topBarrier, bottomBarrier);

    //    // Adjust the position to the center of the screen, now considering clamping
    //    var positionOffset = Matrix.CreateTranslation(
    //        -targetX + (viewportWidth / 2),
    //        -targetY + (viewportHeight / 2),
    //        0);

    //    // Apply zoom
    //    var zoomMatrix = Matrix.CreateScale(new Vector3(zoom, zoom, 1));

    //    // Combine translation and zoom
    //    Transform = positionOffset * zoomMatrix;
    //}

    //public void Update(Vector2 position, int viewportWidth, int viewportHeight)
    //{
    //    // Calculate the effective viewport size after applying zoom
    //    float effectiveViewportWidth = viewportWidth / zoom;
    //    float effectiveViewportHeight = viewportHeight / zoom;

    //    // Clamp position to ensure the camera's view bounds do not exceed the world size
    //    var clampedX = MathHelper.Clamp(position.X, effectiveViewportWidth / 2, 512 - effectiveViewportWidth / 2);
    //    var clampedY = MathHelper.Clamp(position.Y, effectiveViewportHeight / 2, 256 - effectiveViewportHeight / 2);

    //    // Adjust the position to the center of the screen with the clamped values
    //    var positionOffset = Matrix.CreateTranslation(
    //        -clampedX + (viewportWidth / 2),
    //        -clampedY + (viewportHeight / 2),
    //        0);

    //    // Apply zoom
    //    var zoomMatrix = Matrix.CreateScale(new Vector3(zoom, zoom, 1));

    //    // Combine translation and zoom
    //    Transform = positionOffset * zoomMatrix;
    //}
}
