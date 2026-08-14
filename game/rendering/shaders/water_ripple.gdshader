shader_type canvas_item;

uniform vec4 water_color : source_color = vec4(0.08, 0.25, 0.38, 0.75);
uniform vec4 caustic_color : source_color = vec4(0.2, 0.65, 0.85, 0.45);
uniform vec4 foam_color : source_color = vec4(0.5, 0.9, 0.95, 0.8);
uniform float wave_speed = 1.6;
uniform float wave_frequency = 18.0;
uniform float wave_amplitude = 0.04;

varying vec2 world_pos;

void vertex() {
    world_pos = (MODEL_MATRIX * vec4(VERTEX, 0.0, 1.0)).xy;
}

void fragment() {
    // 2:1 isometric coordinate transformation
    vec2 p = UV - vec2(0.5);
    p.y *= 2.0;
    
    // Wave distortion
    float wave1 = sin(world_pos.x * 0.05 + TIME * wave_speed) * cos(world_pos.y * 0.08 + TIME * wave_speed * 0.8);
    float wave2 = cos(world_pos.x * 0.08 - TIME * wave_speed * 0.6) * sin(world_pos.y * 0.05 + TIME * wave_speed * 1.2);
    float ripple = (wave1 + wave2) * 0.5;
    
    // Caustic highlights
    float caustic = smoothstep(0.3, 0.6, ripple);
    vec4 col = mix(water_color, caustic_color, caustic);
    
    // Outer edge foam ring
    float edge_dist = length(p);
    float edge_foam = smoothstep(0.44, 0.48, edge_dist) * (1.0 - smoothstep(0.48, 0.5, edge_dist));
    col = mix(col, foam_color, edge_foam * 0.8);
    
    // Soft outer boundary alpha
    float alpha_mask = 1.0 - smoothstep(0.47, 0.5, edge_dist);
    col.a *= alpha_mask;
    
    COLOR = col;
}
