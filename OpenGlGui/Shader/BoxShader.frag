#version 400

out vec4 FragColor;
//in vec4 gl_FragCoord;
uniform vec2 iResolution;
uniform vec2 transpose;
uniform vec2 size;


uniform sampler2D texture0;


float sdRoundBox(in vec2 p, in vec2 b, in vec4 r)
{
    r.xy = (p.x > 0.0) ? r.xy : r.zw;
    r.x = (p.y > 0.0) ? r.x : r.y;

    vec2 q = abs(p) - b + r.x;
    return min(max(q.x, q.y), 0.0) + length(max(q, 0.0)) - r.x;
}

void main()
{
    //vec2 iResolution = vec2(1600, 900);
    vec2 p = gl_FragCoord.xy - transpose - iResolution / 2;
    vec2 q = size;

    float d = sdRoundBox(p, q, vec4(12.5));

    float isInsideBorder = 1.0 - smoothstep(-3.0, 0.0, d);

    vec3 border = vec3(0.0, 0.25, 0.25);
    vec3 inside = vec3(0.0, 0.25, 0.4) * isInsideBorder;
    vec3 col = border + inside;

    float visibility = max(0., 1. - step(-0.0, d));
    FragColor = vec4(col, visibility);

    //float textOrObject = smoothstep(-1.75, -1.5, -length(texture(texture0, vec2(p.x, -p.y) / 200.0).xyz));

    vec2 texCoords = vec2(p.x / size.x / 2.0, -p.y / size.y / 2.0) - 0.5;
    vec4 texture = texture(texture0, texCoords);
    vec3 texCol = texture.xyz * texture.w;

    vec4 objAndTextCol = vec4(col, visibility) * (1.0 - texture.w) + texture;
    FragColor = objAndTextCol;
}