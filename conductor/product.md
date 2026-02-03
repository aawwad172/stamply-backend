# Stamply Product Definition

## 1. Initial Concept
Stamply is a SaaS digital loyalty platform designed for cafes and shops. It replaces traditional physical stamp cards with digital ones that live in a user's Apple or Google Wallet. The goal is to provide a more convenient, cost-effective, and modern reward system for both tenants (shops) and end-users.

## 2. Target Audience
- **Tenants (Shop Owners/Admins):** Cafe owners and small retail shops looking to increase customer retention via loyalty rewards.
- **End-Users (Customers):** Patrons of shops who want to earn rewards without carrying physical loyalty cards.
- **Super Admins:** The Stamply internal team managing the platform, subscriptions, and tenant onboarding.

## 3. Core Features (MVP)
- **Multi-Tenancy:** A single-database, row-level isolation architecture to support multiple tenants efficiently.
- **Digital Wallet Integration:** Direct integration with Apple and Google Wallet APIs for card generation, updates, and push notifications.
- **Tenant Landing Pages:** Shop-specific web pages where users scan a QR code to quickly add their loyalty card to their wallet.
- **Rich Domain Model:** Business logic encapsulated within entities (DDD), utilizing methods for state transitions (e.g., adding stamps, redeeming rewards).
- **Flexible Reward System:**
    - Fixed stamp goals (e.g., "Buy 10, get 1 free").
    - Expiring stamps or cards to drive frequency.
    - Multiple reward tiers on a single card (e.g., small reward at 5, large at 10).
- **Tenant Dashboard:** Statistics and management tools for tenant admins to track loyalty program performance.
- **Authorization:** Role-Based Access Control (RBAC) to manage permissions for Tenant Admins, Users, and Super Admins.

## 4. Technical Vision
- **Architecture:** Clean Architecture with Domain, Application, Infrastructure, and Presentation layers.
- **API:** Minimal APIs for a high-performance, lightweight web layer.
- **Database:** PostgreSQL with Row-Level Security (RLS) or shared tables with indexed Tenant IDs.
- **Loyalty Mechanism:** QR code scanning by tenant admins to identify users and update their digital cards in real-time.
