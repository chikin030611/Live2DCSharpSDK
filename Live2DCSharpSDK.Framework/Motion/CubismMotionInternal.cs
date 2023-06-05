﻿namespace Live2DCSharpSDK.Framework.Motion;

/// <summary>
/// モーションカーブのセグメントの評価関数。
/// </summary>
/// <param name="points">モーションカーブの制御点リスト</param>
/// <param name="time">評価する時間[秒]</param>
public delegate float csmMotionSegmentEvaluationFunction(CubismMotionPoint[] points, int start, float time);

/// <summary>
/// モーションカーブの種類。
/// </summary>
public enum CubismMotionCurveTarget
{
    /// <summary>
    /// モデルに対して
    /// </summary>
    CubismMotionCurveTarget_Model,
    /// <summary>
    /// パラメータに対して
    /// </summary>
    CubismMotionCurveTarget_Parameter,
    /// <summary>
    /// パーツの不透明度に対して
    /// </summary>
    CubismMotionCurveTarget_PartOpacity
};

/// <summary>
/// モーションカーブのセグメントの種類。
/// </summary>
public enum CubismMotionSegmentType : int
{
    /// <summary>
    /// リニア
    /// </summary>
    CubismMotionSegmentType_Linear = 0,
    /// <summary>
    /// ベジェ曲線
    /// </summary>
    CubismMotionSegmentType_Bezier = 1,
    /// <summary>
    /// ステップ
    /// </summary>
    CubismMotionSegmentType_Stepped = 2,
    /// <summary>
    /// インバースステップ
    /// </summary>
    CubismMotionSegmentType_InverseStepped = 3
};

/// <summary>
/// モーションカーブの制御点。
/// </summary>
public record CubismMotionPoint
{
    /// <summary>
    /// 時間[秒]
    /// </summary>
    public float Time;
    /// <summary>
    /// 値
    /// </summary>
    public float Value;
}

/// <summary>
/// モーションカーブのセグメント。
/// </summary>
public record CubismMotionSegment
{
    /// <summary>
    /// 使用する評価関数
    /// </summary>
    public csmMotionSegmentEvaluationFunction Evaluate;
    /// <summary>
    /// 最初のセグメントへのインデックス
    /// </summary>
    public int BasePointIndex;
    /// <summary>
    /// セグメントの種類
    /// </summary>
    public CubismMotionSegmentType SegmentType;
}

/// <summary>
/// モーションカーブ。
/// </summary>
public record CubismMotionCurve
{
    /// <summary>
    /// カーブの種類
    /// </summary>
    public CubismMotionCurveTarget Type;
    /// <summary>
    /// カーブのID
    /// </summary>
    public string Id;
    /// <summary>
    /// セグメントの個数
    /// </summary>
    public int SegmentCount;
    /// <summary>
    /// 最初のセグメントのインデックス
    /// </summary>
    public int BaseSegmentIndex;
    /// <summary>
    /// フェードインにかかる時間[秒]
    /// </summary>
    public float FadeInTime;
    /// <summary>
    /// フェードアウトにかかる時間[秒]
    /// </summary>
    public float FadeOutTime;
}

/// <summary>
/// イベント。
/// </summary>
public record CubismMotionEvent
{
    public float FireTime;
    public string Value;
}

/// <summary>
/// モーションデータ。
/// </summary>
public record CubismMotionData
{
    /// <summary>
    /// モーションの長さ[秒]
    /// </summary>
    public float Duration;
    /// <summary>
    /// ループするかどうか
    /// </summary>
    public bool Loop;
    /// <summary>
    /// カーブの個数
    /// </summary>
    public int CurveCount;
    /// <summary>
    /// UserDataの個数
    /// </summary>
    public int EventCount;
    /// <summary>
    /// フレームレート
    /// </summary>
    public float Fps;
    /// <summary>
    /// カーブのリスト
    /// </summary>
    public CubismMotionCurve[] Curves;
    /// <summary>
    /// セグメントのリスト
    /// </summary>
    public CubismMotionSegment[] Segments;
    /// <summary>
    /// ポイントのリスト
    /// </summary>
    public CubismMotionPoint[] Points;
    /// <summary>
    /// イベントのリスト
    /// </summary>
    public CubismMotionEvent[] Events;
}