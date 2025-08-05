# ⛵ HR Tracker Function

**HallbergRassyTracker** is an Azure Function project designed to log and retrieve sightings of Hallberg-Rassy sailboats during a sailing trip. It exposes two HTTP endpoints for storing and fetching boat data in Azure Table Storage.

This project was created as part of a technical interview test — and inspired by real-life sailing aboard a Hallberg-Rassy 352 from 1979.

---

## 🚀 Features

- **POST /api/boats**  
  Accepts a JSON payload representing a Hallberg-Rassy boat and stores it in Azure Table Storage.

- **GET /api/boats**  
  Returns all stored boat entries.

---

## 📦 Example JSON Payload

```json
{
  "boatBrand": "Hallberg-Rassy",
  "model": "352",
  "lengthFeet": 35,
  "yearBuilt": 1979,
  "locationSpotted": "Ellös, Sweden",
  "dateSpotted": "2025-08-05"
}
