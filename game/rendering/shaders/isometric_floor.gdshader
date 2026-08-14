shader_type canvas_item;

uniform vec4 base_color : source_color = vec4(0.08, 0.09, 0.12, 1.0);
uniform vec4 tile_color : source_color = vec4(0.12, 0.14, 0.18, 1.0);
uniform vec4 grid_line_color : source_color = vec4(0.18, 0.22, 0.28, 1.0);
uniform vec4 conduit_glow_color : source_color = vec4(1.0, 0.55, 0.15, 0.4);
uniform vec2 tile_size = vec2(64.0, 32.0);
uniform float line_width = 1.0;
uniform float conduit_speed = 0.8;

varying vec2 world_pos;

// Pseudo-random noise helper
float hash(vec2 p) {
    p = fract(p * vec2(123.34, 456.21));
    p += dot(p, p + 45.32);
    return fract(p.x * p.y);
}

void vertex() {
    world_pos = (MODEL_MATRIX * vec4(VERTEX, 0.0, 1.0)).xy;
}

void fragment() {
    // 2:1 Isometric grid transformation
    vec2 iso_coords;
    iso_coords.x = (world_pos.x / tile_size.x) + (world_pos.y / tile_size.y);
    iso_coords.y = (world_pos.y / tile_size.y) - (world_pos.x / tile_size.x);
    
    vec2 cell_index = floor(iso_coords);
    vec2 cell_fract = fract(iso_coords);
    
    // Grid border distance
    vec2 dist_to_edge = min(cell_fract, 1.0 - cell_fract);
    float min_dist = min(dist_to_edge.x, dist_to_edge.y);
    
    // Tile variation noise
    float tile_noise = hash(cell_index) * 0.08;
    vec4 current_tile = tile_color + vec4(tile_noise, tile_noise, tile_noise, 0.0);
    
    // Grid line antialiasing
    float grid_mask = smoothstep(0.0, 0.04, min_dist);
    vec4 col = mix(grid_line_color, current_tile, grid_mask);
    
    // Subtle animated ember pulse along conduit lines
    if (mod(cell_index.x, 6.0) < 1.0 || mod(cell_index.y, 6.0) < 1.0) {
        float pulse = sin(TIME * conduit_speed + (cell_index.x + cell_index.y) * 0.5) * 0.5 + 0.5;
        col = mix(col, col + conduit_glow_color * pulse, 0.3);
    }
    
    COLOR = col;
}
