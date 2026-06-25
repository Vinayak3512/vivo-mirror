'use client';

import { useMemo, useState } from "react";
import Link from "next/link";

type Announcement = {
  id: string;
  title: string;
  category: "hr-update" | "event" | "policy" | "celebration" | "compliance" | "general";
  priority: "low" | "medium" | "high";
  content: string;
  postedDate: string;
  expiryDate: string;
  views: number;
  likes: number;
  acknowledged: boolean;
};

function generateMockData(): Announcement[] {
  const data: Announcement[] = [];
  const categories: Announcement["category"][] = [
    "hr-update",
    "event",
    "policy",
    "celebration",
    "compliance",
    "general",
  ];
  const priorities: Announcement["priority"][] = ["low", "medium", "high"];
  const titles = [
    "New Leave Policy Effective from July 1st",
    "Company Outing - Beach Day!",
    "Updated Work from Home Guidelines",
    "Celebrating Q2 Success",
    "Mandatory Compliance Training",
    "Office Renovation Complete",
  ];
  const contents = [
    "Please review the updated leave policy in the HR portal.",
    "Join us for a fun day at the beach on Saturday!",
    "New WFH guidelines have been implemented.",
    "Great quarter! Thanks for your hard work.",
    "All employees must complete the training by end of month.",
    "Our office renovation is now complete. Welcome to the new space!",
  ];

  for (let i = 0; i < 6; i++) {
    const postedDate = new Date();
    postedDate.setDate(postedDate.getDate() - i * 2);
    const expiryDate = new Date(postedDate);
    expiryDate.setDate(expiryDate.getDate() + 30);

    data.push({
      id: `announcement-${i}`,
      title: titles[i],
      category: categories[i % categories.length],
      priority: priorities[i % priorities.length],
      content: contents[i],
      postedDate: postedDate.toISOString().split("T")[0],
      expiryDate: expiryDate.toISOString().split("T")[0],
      views: Math.floor(Math.random() * 500) + 50,
      likes: Math.floor(Math.random() * 100) + 10,
      acknowledged: Math.random() > 0.5,
    });
  }

  return data;
}

const categoryColors: Record<Announcement["category"], string> = {
  "hr-update": "bg-blue-100 text-blue-800",
  event: "bg-purple-100 text-purple-800",
  policy: "bg-orange-100 text-orange-800",
  celebration: "bg-pink-100 text-pink-800",
  compliance: "bg-red-100 text-red-800",
  general: "bg-gray-100 text-gray-800",
};

const priorityColors: Record<Announcement["priority"], string> = {
  low: "border-l-4 border-green-500",
  medium: "border-l-4 border-yellow-500",
  high: "border-l-4 border-red-500",
};

export default function AnnouncementsPage() {
  const data = useMemo(() => generateMockData(), []);
  const [filter, setFilter] = useState<"all" | "unacknowledged">("all");

  const filteredData =
    filter === "unacknowledged"
      ? data.filter((a) => !a.acknowledged)
      : data;

  return (
    <div className="min-h-screen bg-gradient-to-br from-teal-50 to-orange-50 dark:from-slate-900 dark:to-slate-800">
      {/* Header */}
      <header className="border-b border-teal-200 bg-white shadow-sm dark:border-slate-700 dark:bg-slate-800">
        <div className="mx-auto max-w-4xl px-4 py-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between">
            <div>
              <Link href="/dashboard" className="text-sm text-teal-600 hover:text-teal-700 dark:text-teal-400">
                ← Back to Dashboard
              </Link>
              <h1 className="mt-2 text-2xl font-bold text-slate-900 dark:text-white">
                Announcements
              </h1>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="mx-auto max-w-4xl px-4 py-8 sm:px-6 lg:px-8">
        {/* Filter Tabs */}
        <div className="mb-6 flex gap-4">
          <button
            onClick={() => setFilter("all")}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              filter === "all"
                ? "bg-teal-600 text-white"
                : "bg-white text-slate-700 border border-slate-200 hover:bg-slate-50 dark:bg-slate-700 dark:text-slate-300 dark:border-slate-600"
            }`}
          >
            All Announcements ({data.length})
          </button>
          <button
            onClick={() => setFilter("unacknowledged")}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              filter === "unacknowledged"
                ? "bg-orange-600 text-white"
                : "bg-white text-slate-700 border border-slate-200 hover:bg-slate-50 dark:bg-slate-700 dark:text-slate-300 dark:border-slate-600"
            }`}
          >
            Pending Acknowledgment ({data.filter((a) => !a.acknowledged).length})
          </button>
        </div>

        {/* Announcements List */}
        <div className="space-y-4">
          {filteredData.map((announcement) => (
            <div
              key={announcement.id}
              className={`rounded-lg bg-white p-6 shadow-md transition-all hover:shadow-lg dark:bg-slate-700 ${
                priorityColors[announcement.priority]
              }`}
            >
              <div className="flex items-start justify-between gap-4">
                <div className="flex-1">
                  <div className="flex items-center gap-3">
                    <h3 className="text-lg font-bold text-slate-900 dark:text-white">
                      {announcement.title}
                    </h3>
                    <span
                      className={`rounded-full px-3 py-1 text-xs font-medium ${
                        categoryColors[announcement.category]
                      }`}
                    >
                      {announcement.category.replace("-", " ").toUpperCase()}
                    </span>
                  </div>
                  <p className="mt-2 text-slate-600 dark:text-slate-400">
                    {announcement.content}
                  </p>
                  <div className="mt-4 flex items-center gap-4 text-sm text-slate-500 dark:text-slate-400">
                    <span>
                      Posted: {new Date(announcement.postedDate).toLocaleDateString()}
                    </span>
                    <span>Expires: {new Date(announcement.expiryDate).toLocaleDateString()}</span>
                    <span>👁 {announcement.views} views</span>
                    <span>❤ {announcement.likes} likes</span>
                  </div>
                </div>
                <div className="flex flex-col gap-2">
                  {!announcement.acknowledged ? (
                    <button className="rounded-lg bg-orange-600 px-4 py-2 text-sm font-medium text-white hover:bg-orange-700">
                      Acknowledge
                    </button>
                  ) : (
                    <span className="rounded-lg bg-green-100 px-4 py-2 text-sm font-medium text-green-800 dark:bg-green-900 dark:text-green-200">
                      ✓ Acknowledged
                    </span>
                  )}
                </div>
              </div>
            </div>
          ))}
        </div>

        {filteredData.length === 0 && (
          <div className="rounded-lg bg-white p-12 text-center dark:bg-slate-700">
            <p className="text-slate-600 dark:text-slate-400">
              No announcements to display.
            </p>
          </div>
        )}
      </main>
    </div>
  );
}
