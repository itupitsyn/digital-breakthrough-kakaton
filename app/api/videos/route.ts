import { Video } from "@/model/video";
import { faker } from "@faker-js/faker";
import { NextResponse } from "next/server";

const data: Video[] = [];
for (let i = 0; i < 90; i += 1) {
  const randomObj = {
    id: faker.string.alphanumeric(),
    name: faker.word.words({ count: Math.ceil(Math.random() * 10) }),
    description: faker.lorem.paragraph(),
    preview: faker.image.urlLoremFlickr(),
  };

  data.push(randomObj);
}

export const GET = () => {
  return new NextResponse(JSON.stringify(data));
};
