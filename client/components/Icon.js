import L from "leaflet";

const markerIcon = new L.Icon({
  iconUrl: "/vtb.svg",
  iconSize: [32, 32],
  iconAnchor: [16, 16],
});
const userIcon = new L.Icon({
  iconUrl: 'https://unpkg.com/leaflet@1.9.3/dist/images/marker-icon-2x.png',
  iconSize: [25, 41],
  iconAnchor: [12.5, 20.5],
});

export { markerIcon, userIcon };
