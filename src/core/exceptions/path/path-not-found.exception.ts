import { NotFoundException } from "@exceptions/base/not-found.exception";

export class PathNotFoundException extends NotFoundException {
  constructor(path: string) {
    super("File or directory", "path", path);
  }
}
