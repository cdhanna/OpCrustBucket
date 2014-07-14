using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SmallNet.Samples
{
    class Camera2D
    {
        #region  Instance Data and Constructors
 
        private Vector2 _position;
        private Vector2 _origin;
        private float _scale;
        private float _rotation;
        private float _rotationChange;
        private Matrix _matrix;

        private Vector2 _screenSize;
        private Vector2 _screenCenter;

        private bool _transitionInProgress;
        private bool _lockAttributes;
        private Vector2 _targetPosition;
        private Vector2 _targetOrigin;
        private float _targetScale;
        private float _targetRotation;
        private double _duration;
        private double _currentTime;
        

        /// <summary>
        /// Constructor for the Camera2D class
        /// </summary>
        public Camera2D(Vector2 screenSize)
        {
            _origin = Vector2.Zero;
            _position = Vector2.Zero;
            _scale = 1;
            _rotation = 0;
            UpdateTranslation();
            _screenCenter = screenSize / 2;
            _screenSize = screenSize;
        }

        /// <summary>
        /// Constructor for the Camera2D class
        /// </summary>
        /// <param name="NewOrigin">Starting origin (Screen coordinates)</param>
        /// <param name="NewPosition">Starting position</param>
        /// <param name="NewScale">Starting zoom value</param>
        /// <param name="NewRotation">Starting rotation</param>
        public Camera2D(Vector2 NewOrigin, Vector2 NewPosition, float NewScale, float NewRotation, Vector2 screenSize)
        {
            _origin = Vector2.Zero;
            OriginSet(NewOrigin);
            _position += NewPosition;
            _scale = NewScale;
            _rotation = NewRotation;
            UpdateTranslation();
            _lockAttributes = false;
            _screenCenter = screenSize / 2;
            _screenSize = screenSize;
        }

        #endregion

        #region  Public Accessors
        public Vector2 Origin { get { return _origin; } set { OriginSet(value); } }
        public Vector2 Position { get { return _position; } set { PositionSet(value); } }
        public float Scale { get { return _scale; } set { ZoomSet(value); } }
        public float Rotation { get { return _rotation; } set { RotatationSet(value); } }
        public Matrix Translation { get { return _matrix; } }

        #endregion

        #region  Zoom Methods 

        /// <summary>
        /// Set the zoom level
        /// </summary>
        /// <param name="scale">New zoom level</param>
        public void ZoomSet(float scale)
        {
            if (!_lockAttributes)
            {
                if (scale >= 0.05f && scale <= 4.0f)
                {
                    _scale = scale;
                }
                else
                {
                    if (scale < .05f)
                    {
                        _scale = .05f;
                    }
                    else
                    {
                        _scale = 4.0f;
                    }
                }

                UpdateTranslation();
            }
        }

        /// <summary>
        /// Set the zoom level
        /// </summary>
        /// <param name="zoomCenterOnScreen">Position to zoom around</param>
        /// <param name="scale">New zoom level</param>
        public void ZoomSet(Vector2 zoomCenterOnScreen, float scale)
        {
            if (!_lockAttributes)
            {
                _origin = zoomCenterOnScreen;
                _scale = scale;
                UpdateTranslation();
            }
        }

        /// <summary>
        /// Increase the zoom level
        /// </summary>
        /// <param name="amount">amount to zoom by</param>
        public void ZoomIncrease(float amount)
        {
            if (!_lockAttributes)
            {
                if (_scale + amount < 4.00f)
                {
                    _scale += amount;
                }
                else
                {
                    _scale = 4.00f;
                }

                UpdateTranslation();
            }
        }

        /// <summary>
        /// Increase the zoom level
        /// </summary>
        /// <param name="zoomCenterOnScreen">Screen Position to zoom around</param>
        /// <param name="amount">Amount to zoom by</param>
        public void ZoomIncrease(Vector2 zoomCenterOnScreen, float amount)
        {
            if (!_lockAttributes)
            {
                OriginSet(zoomCenterOnScreen);
                if (_scale + amount < 4.00f)
                {
                    _scale += amount;
                }
                else
                {
                    _scale = 4.00f;
                }

                UpdateTranslation();
            }
        }

        /// <summary>
        /// Decrease the zoom level
        /// </summary>
        /// <param name="amount">amount to zoom by</param>
        public void ZoomDecrease(float amount)
        {
            if (!_lockAttributes)
            {
                if (_scale - amount > 0.10f)
                {
                    _scale -= amount;
                }
                else
                {
                    _scale = 0.10f;
                }
                UpdateTranslation();
            }
        }

        /// <summary>
        /// Decrease the zoom level
        /// </summary>
        /// <param name="zoomCenterOnScreen">Screen Position to zoom around</param>
        /// <param name="amount">Amount to zoom by</param>
        public void ZoomDecrease(Vector2 zoomCenterOnScreen, float amount)
        {
            if (!_lockAttributes)
            {
                OriginSet(zoomCenterOnScreen);

                if (_scale - amount > 0.10f)
                {
                    _scale -= amount;
                }
                else
                {
                    _scale = 0.10f;
                }
                UpdateTranslation();
            }
        }

        #endregion

        #region  Position Methods 

        public void PositionChangeBy(Vector2 change)
        {
            if (!_lockAttributes)
            {
                _position += change;
                UpdateTranslation();
            }
        }

        public void PositionSet(Vector2 screenCoordinates)
        {
            if (!_lockAttributes)
            {
                _position = screenCoordinates;
                UpdateTranslation();
            }
        }

        public void PositionMoveLeft(float amount)
        {
            if (!_lockAttributes)
            {
                _position += new Vector2(amount, 0);
                UpdateTranslation();
            }
        }

        public void PositionMoveRight(float amount)
        {
            if (!_lockAttributes)
            {
                _position -= new Vector2(amount, 0);
                UpdateTranslation();
            }
        }

        public void PositionMoveUp(float amount)
        {
            if (!_lockAttributes)
            {
                _position += new Vector2(0, amount);
                UpdateTranslation();
            }
        }

        public void PositionMoveDown(float amount)
        {
            if (!_lockAttributes)
            {
                _position -= new Vector2(0, amount);
                UpdateTranslation();
            }
        }

        #endregion

        #region  Origin Reposition Methods 
        
        /// <summary>
        /// Change the position of the origin by a specified amount
        /// </summary>
        /// <param name="change">The position change for the origin</param>
        public void OriginChangeBy(Vector2 change)
        {
            if (!_lockAttributes)
            {
                _origin += change;
                UpdateTranslation();
            }
        }

        /// <summary>
        /// Updates the Origin to the specific coordinates provided
        /// </summary>
        /// <param name="screenLocation">The location on the screen to where the location should be set</param>
        public void OriginSet(Vector2 screenLocation)
        {
            if (!_lockAttributes)
            {
                Vector2 originNew = GetGameLocation(screenLocation);
                _origin = originNew;
                UpdateTranslation();
                originNew = GetScreenLocation(_origin);
                _position += (screenLocation - originNew);
                UpdateTranslation();
            }
        }

        /// <summary>
        /// Move the origin to the left.
        /// </summary>
        /// <param name="amount">Amount to move the origin by</param>
        public void OriginMoveLeft(float amount)
        {
            if (!_lockAttributes)
            {
                _origin -= new Vector2(amount, 0);
                UpdateTranslation();
            }
        }

        /// <summary>
        /// Move the origin to the right.
        /// </summary>
        /// <param name="amount">Amount to move the origin by</param>
        public void OriginMoveRight(float amount)
        {
            if (!_lockAttributes)
            {
                _origin += new Vector2(amount, 0);
                UpdateTranslation();
            }
        }

        /// <summary>
        /// Move the origin up.
        /// </summary>
        /// <param name="amount">Amount to move the origin by</param>
        public void OriginMoveUp(float amount)
        {
            if (!_lockAttributes)
            {
                _origin -= new Vector2(0, amount);
                UpdateTranslation();
            }
        }

        /// <summary>
        /// Move the origin down.
        /// </summary>
        /// <param name="amount">Amount to move the origin by</param>
        public void OriginMoveDown(float amount)
        {
            if (!_lockAttributes)
            {
                _origin += new Vector2(0, amount);
                UpdateTranslation();
            }
        }
        #endregion

        #region  Rotation Methods 

        /// <summary>
        /// Set the rotation of the camera
        /// </summary>
        /// <param name="angle">The new rotation (in radians)</param>
        public void RotatationSet(float angle)
        {
            if (!_lockAttributes)
            {
                _rotation = angle;
                UpdateTranslation();
            }
        }

        /// <summary>
        /// Set the rotation of the camera
        /// </summary>
        /// <param name="angle">The new rotation (in degrees)</param>
        public void RotatationSetDegrees(float angle)
        {
            if (!_lockAttributes)
            {
                _rotation = MathHelper.ToRadians(angle);
                UpdateTranslation();
            }
        }

        /// <summary>
        /// Increase or decrease the rotation by a specific value
        /// </summary>
        /// <param name="rotationChange">The amount to change the rotation by (in radians)</param>
        public void RotateBy(float rotationChange)
        {
            if (!_lockAttributes)
            {
                _rotation = MathHelper.WrapAngle(_rotation + rotationChange);
                _rotationChange = rotationChange;
                UpdateTranslation();
            }
        }

        /// <summary>
        /// Increase or decrease the rotation by a specific value
        /// </summary>
        /// <param name="rotationChange">The amount to change the rotation by (in degrees)</param>
        public void RotateByDegrees(float rotationChange)
        {
            if (!_lockAttributes)
            {
                _rotation = _rotation = MathHelper.WrapAngle(_rotation + MathHelper.ToRadians(rotationChange));
                _rotationChange = MathHelper.ToRadians(rotationChange);
                UpdateTranslation();
            }
        }

        #endregion

        #region  Helper Methods 

        /// <summary>
        /// Helper method to convert a Vector2 to a Vector3 for Matrix translations
        /// </summary>
        /// <param name="v2">Vector2 to convert</param>
        /// <returns>A Vector3 based off the Vector2 provided with a z-value of zero</returns>
        private Vector3 v3(Vector2 v2)
        {
            return new Vector3(v2, 0);
        }
        
        /// <summary>
        /// Helper method to round a Vector2
        /// </summary>
        /// <param name="v2">The vector to be rounded</param>
        /// <returns>Rounded vector2</returns>
        private Vector2 round(Vector2 v2)
        {
            return new Vector2((float)Math.Round((double)v2.X), (float)Math.Round((double)v2.Y));
        }

        /// <summary>
        /// Convert a location on the screen to the game location in that same screen position.
        /// </summary>
        /// <param name="screenPosition">The position on the screen</param>
        /// <returns>Vector2 of the location of that point on the screen in the game grid</returns>
        public Vector2 GetGameLocation(Vector2 screenPosition)
        {
            Matrix translation;
            if (!_transitionInProgress)
            {
                translation = Matrix.CreateTranslation(v3(-_position)) *
                    Matrix.CreateTranslation(v3(-_origin)) *
                    Matrix.CreateRotationZ(-_rotation) *
                    Matrix.CreateScale(1 / _scale) *
                    Matrix.CreateTranslation(v3(_origin));


            }
            else
            {
                translation = Matrix.CreateTranslation(CalculateTranslation(-_position, -_targetPosition)) *
                    Matrix.CreateTranslation(CalculateTranslation(-_origin, -_targetOrigin)) *
                    Matrix.CreateRotationZ(CalculateTranslation(-_rotation, -_targetRotation)) *
                    Matrix.CreateScale(CalculateTranslation(1 / _scale, 1 / _targetScale)) *
                    Matrix.CreateTranslation(CalculateTranslation(_origin, _targetOrigin));
            }

            Vector2 gameLocation = Vector2.Transform(screenPosition, translation);
            return gameLocation;
        }

        public static Vector2 GetGameLocation(Vector2 screenPosition, Camera2D camera)
        {
            Matrix translation;

                translation = Matrix.CreateTranslation(camera.v3(-camera.Position)) *
                    Matrix.CreateTranslation(camera.v3(-camera.Origin)) *
                    Matrix.CreateRotationZ(-camera.Rotation) *
                    Matrix.CreateScale(1 / camera.Scale) *
                    Matrix.CreateTranslation(camera.v3(camera.Origin));


            Vector2 gameLocation = Vector2.Transform(screenPosition, translation);
            return gameLocation;
        }

        /// <summary>
        /// Get the location on the screen of a given point in the game coordinates
        /// </summary>
        /// <param name="gamePosition">Game coordinates of the point to convert</param>
        /// <returns>Vector2 of the screen location for the given game location</returns>
        public Vector2 GetScreenLocation(Vector2 gamePosition)
        {
            return Vector2.Transform(gamePosition, _matrix);
        }

        /// <summary>
        /// Resets the camera to its default values
        /// </summary>
        public void Reset()
        {
            _origin = Vector2.Zero;
            _position = Vector2.Zero;
            _rotation = 0;
            _scale = 1;

            UpdateTranslation();
        }

        /// <summary>
        /// Updates the translation matrix.  Called when changes to values are made.
        /// </summary>
        private void UpdateTranslation()
        {
            _matrix = Matrix.Identity * 
                Matrix.CreateTranslation(v3(-_origin)) * 
                Matrix.CreateRotationZ(_rotation) * 
                Matrix.CreateScale(_scale) * 
                Matrix.CreateTranslation(v3(_position)) * 
                Matrix.CreateTranslation(v3(_origin));

            _origin = round(_origin);
            _scale = (float)Math.Round(_scale, 3);
            _position = round(_position);
        }

        /// <summary>
        /// Provides diagnostic information including the states of all relevant variables
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Origin: " + _origin +
                "\nOrigin on Screen: " + GetScreenLocation(_origin) +
                "\nPosition: " + _position +
                "\nZoom: " + _scale +
                "\nRotation: " + _rotation +
                "\nTransition in progress: " + _transitionInProgress + 
                "\nTransition Duration: " + _duration +
                "\nCurrent time: " + _currentTime;

        }

        #endregion


        public void PositionForPoints(Vector2[] gamePoints)
        {
            Vector2 average = Vector2.Zero;
            float extraLength = 0;


            foreach (Vector2 v in gamePoints)
                average += v;
            average /= gamePoints.Length;


            foreach (Vector2 v in gamePoints)
            {
                float testExtra = (average - v).Length();
                if (testExtra > extraLength) extraLength = testExtra;
            }
            extraLength *= 1.5f*1.5f;



            Vector2 positionNew = average;
            Origin = (GetScreenLocation(-Position + new Vector2(720, 450)));
            
            PositionSet(-positionNew);


            float s = 500 / extraLength ;
           
            float xboxSMod = 1f;

            s *= xboxSMod;
            if (s < .133f * xboxSMod) s = .133f * xboxSMod;
            if (s > .5f * xboxSMod + (.1f * gamePoints.Length)) s = .5f * xboxSMod + (.1f * gamePoints.Length);
            
            ZoomSet(s);



           // UpdateTranslation();

        }

        public void TransitionStart(Vector2 origin, Vector2 position, float rotation, float scale, double duration)
        {

            _lockAttributes = true;
            _transitionInProgress = true;
            _targetOrigin = origin;
            _targetPosition = position;
            _targetRotation = rotation;
            _targetScale = scale;
            _currentTime = 0;
            _duration = duration;

        }

        public void SlideLeft(float amount, double duration)
        {
            TransitionStart(_origin, _position - new Vector2(amount, 0), _rotation, _scale, duration);
        }

        public void SlideRight(float amount, double duration)
        {
            TransitionStart(_origin, _position + new Vector2(amount, 0), _rotation, _scale, duration);
        }

        /// <summary>
        /// Predefined transition
        /// Shift the map up
        /// </summary>
        /// <param name="amount">Amount to move the map up by</param>
        /// <param name="duration">Amount of time for the effect</param>
        public void SlideUp(float amount, double duration)
        {
            TransitionStart(_origin, _position - new Vector2(0, amount), _rotation, _scale, duration);
        }

        /// <summary>
        /// Predefined transition
        /// Shift the map down
        /// </summary>
        /// <param name="amount">Amount to move the map down by</param>
        /// <param name="duration">Amount of time for the effect</param>
        public void SlideDown(float amount, double duration)
        {
            TransitionStart(_origin, _position + new Vector2(0, amount), _rotation, _scale, duration);
        }

        /// <summary>
        /// Predefined transition
        /// Spins around the origin a specific amount of times
        /// </summary>
        /// <param name="rotations">The amount of times to rotate around the origin</param>
        /// <param name="duration">Amount of time for the effect</param>
        public void Spin(double rotations, double duration)
        {
            TransitionStart(_origin, _position, (_rotation + (float)(rotations * MathHelper.TwoPi)), 1, duration);
        }

        /// <summary>
        /// Predefined transition
        /// Spins around the origin a specific number of radians
        /// </summary>
        /// <param name="rotations">The amount rotation around the origin</param>
        /// <param name="duration">Amount of time for the effect</param>
        public void SpinBy(float rotations, double duration)
        {
            TransitionStart(_origin, _position, _rotation + rotations, 1, duration);
        }

        /// <summary>
        /// Predefined Transition
        /// Zooms in on a specific point, moving to the center of the screen
        /// </summary>
        /// <param name="position">Game location of the zoom center</param>
        /// <param name="zoom">Amount to zoom by</param>
        /// <param name="duration">Amount of time for the effect</param>
        public void ZoomTo(Vector2 position, float zoom, double duration)
        {
            Vector2 targetPosition = _screenCenter - position;
            TransitionStart(position, targetPosition, _rotation, zoom, duration);
        }

        /// <summary>
        /// Helper method to transition value for two Vector2s
        /// </summary>
        /// <param name="start">Starting value</param>
        /// <param name="end">Ending value</param>
        private Vector3 CalculateTranslation(Vector2 start, Vector2 end)
        {
            float x;
            float y;
            x = MathHelper.SmoothStep(start.X, end.X, (float) (_currentTime / _duration));
            y = MathHelper.SmoothStep(start.Y, end.Y, (float) (_currentTime / _duration));

            return new Vector3(x, y, 0);
        }

        /// <summary>
        /// Helper method to calculate the transition value for two floats
        /// </summary>
        /// <param name="start">Starting Value</param>
        /// <param name="end">Ending Value</param>
        private float CalculateTranslation(float start, float end)
        {
            return MathHelper.SmoothStep(start, end, (float)(_currentTime / _duration));
        }

        /// <summary>
        /// Updates the transitions
        /// </summary>
        /// <param name="time">Current game time</param>
        public void Update(GameTime time)
        {
            
            if (_transitionInProgress)
            {
                _currentTime += time.ElapsedGameTime.TotalSeconds;

                if (_currentTime <= _duration)
                {
                    
                    _matrix = Matrix.Identity *
                        Matrix.CreateTranslation(CalculateTranslation(-_origin, -_targetOrigin)) *
                        Matrix.CreateRotationZ(CalculateTranslation(_rotation, _targetRotation)) *
                        Matrix.CreateScale(CalculateTranslation(_scale, _targetScale)) *
                        Matrix.CreateTranslation(CalculateTranslation(_position, _targetPosition)) *
                        Matrix.CreateTranslation(CalculateTranslation(_origin, _targetOrigin));
                }
                else
                {

                    _origin = _targetOrigin;
                    _position = _targetPosition;
                    _rotation = _targetRotation;
                    _scale = _targetScale;
                    _transitionInProgress = false;
                    _lockAttributes = false;
                    _duration = 0.0f;
                    _currentTime = 0.0f;
                }
            }
        }
    }
}
